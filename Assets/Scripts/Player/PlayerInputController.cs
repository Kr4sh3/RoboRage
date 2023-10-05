using UnityEngine;
using UnityEngine.InputSystem;

public enum InputState
{
    DefaultState,
    InventoryState
}

[RequireComponent(typeof(SideScrollerMovementController))]
public class PlayerInputController : MonoBehaviour
{
    #region Instance
    public static PlayerInputController Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>();
            return _instance;
        }
    }
    private static PlayerInputController _instance;
    #endregion

    private InputState _inputState;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _bulletLifeTime;
    private bool _attacking;
    public Vector2 MouseAim;
    public bool FacingDirection { get { return _moveController.FacingDirection; } }
    public int LookDirection { get { return _moveController.LookDirection; } }
    private InputMaster _controls;
    private Camera _cameraMain;
    [SerializeField] GameObject _bulletPrefab;
    private float _attackTimer = 0;
    private Animator _anim;

    private SideScrollerMovementController _moveController;

    private void Start()
    {
        _moveController = GetComponent<SideScrollerMovementController>();
        _cameraMain = Camera.main;
        _anim = GetComponentInChildren<Animator>();

        _controls = new InputMaster();
        _controls.Enable();


        _controls.Player.Attack.performed += ctx => { if (!UIManager.Instance.IsPaused()) AttackPerformed(); };
        _controls.Player.Attack.canceled += ctx => { if (!UIManager.Instance.IsPaused()) AttackCanceled(); };
        _controls.Player.Movement.performed += ctx => { _moveController.SetInputs(ctx.ReadValue<Vector2>()); };
        _controls.Player.AimMouse.performed += ctx => { if (!UIManager.Instance.IsPaused()) MouseAim = ctx.ReadValue<Vector2>(); };
        _controls.Player.Cancel.performed += ctx => { if (!UIManager.Instance.IsPaused()) CancelPerformed(ctx.control); };
        _controls.Player.Inventory.performed += ctx => { if (!UIManager.Instance.IsPaused()) InventoryPerformed(); };
        _controls.Player.Jump.performed += ctx => { if (!UIManager.Instance.IsPaused()) _moveController.SetJumpButton(true); _moveController.Jump(); };
        _controls.Player.Jump.canceled += ctx => { if (!UIManager.Instance.IsPaused()) _moveController.SetJumpButton(false); };
        _controls.Player.Drop.performed += ctx => { if (!UIManager.Instance.IsPaused()) DropPerformed(); };
        _controls.Player.Pause.performed += ctx => { UIManager.Instance.Pause(); };
        _controls.Player.DevMode.performed += ctx => { if (Application.isEditor) UpgradeController.devMode = !UpgradeController.devMode; };
    }

    public void DestroyControls()
    {
        _controls.Dispose();
    }

    private void DropPerformed()
    {
        switch (_inputState)
        {
            case InputState.DefaultState:
                break;
            case InputState.InventoryState:
                UIManager.Instance.Drop();
                break;
        }
    }

    private void AttackPerformed()
    {
        switch (_inputState)
        {
            case InputState.DefaultState:
                _attacking = true;
                break;
            case InputState.InventoryState:
                if (UIManager.Instance.SelectedElements.Count == 0 && InventoryManager.Instance.HeldItem == null)
                    _attacking = true;
                else
                    UIManager.Instance.Submit();
                break;
        }
    }

    private void InventoryPerformed()
    {
        switch (_inputState)
        {
            case InputState.DefaultState:
                SwitchState(InputState.InventoryState);
                UIManager.Instance.OpenPlayerInventory();
                break;
            case InputState.InventoryState:
                SwitchState(InputState.DefaultState);
                UIManager.Instance.ClosePlayerInventory();
                break;
        }
    }

    private void AttackCanceled()
    {
        switch (_inputState)
        {
            case InputState.DefaultState:
                _attacking = false;
                break;
            case InputState.InventoryState:
                _attacking = false;
                break;
        }
    }

    private void CancelPerformed(InputControl control)
    {
        switch (_inputState)
        {
            case InputState.DefaultState:
                break;
            case InputState.InventoryState:
                UIManager.Instance.Cancel();
                break;
        }
    }

    private void SwitchState(InputState state)
    {
        _inputState = state;
    }

    private void Update()
    {

        HeldItemRenderer.Instance.SetTarget(_cameraMain.ScreenToWorldPoint(new Vector3(MouseAim.x, MouseAim.y, 10)));

        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
            return;
        }

        if (!_attacking)
            return;

        BulletController bullet;
        //Instantiate Bullet
        if (_moveController.LookDirection == 1)
        {
            bullet = Instantiate(_bulletPrefab, transform.position + new Vector3(0, .5f, 0), Quaternion.identity).GetComponent<BulletController>();
            _anim.SetTrigger("Aim");
            _anim.SetInteger("AimDir", 1);
        }
        else
        if (_moveController.LookDirection == -1)
        {
            bullet = Instantiate(_bulletPrefab, transform.position + new Vector3(0, -.1f, 0), Quaternion.identity).GetComponent<BulletController>();
            _moveController.RecoilNudge(new Vector2(0, 1));
            _anim.SetTrigger("Aim");
            _anim.SetInteger("AimDir", -1);
        }
        else
        if (_moveController.FacingDirection)
        {
            bullet = Instantiate(_bulletPrefab, transform.position + new Vector3(0.3f, .3f, 0), Quaternion.identity).GetComponent<BulletController>();
            _anim.SetTrigger("Aim");
            _anim.SetInteger("AimDir", 0);
        }
        else
        {
            bullet = Instantiate(_bulletPrefab, transform.position + new Vector3(-0.3f, .3f, 0), Quaternion.identity).GetComponent<BulletController>();
            _anim.SetTrigger("Aim");
            _anim.SetInteger("AimDir", 0);
        }
        VolatileSound.Create(AssetManager.Instance.ShootSound);
        _attackTimer = _attackCooldown;
        bullet.bulletDamage = _attackDamage;
        bullet.bulletSpeed = _bulletSpeed;
        bullet.bulletLife = _bulletLifeTime;
    }

    public void AddPower(int amount)
    {
        _attackDamage += amount;
    }

    public void AddFireRate()
    {
        _attackCooldown -= .1f;
    }

    public void SetRange(float speed, float lifetime)
    {
        _bulletSpeed = speed;
        _bulletLifeTime = lifetime;
    }

}
