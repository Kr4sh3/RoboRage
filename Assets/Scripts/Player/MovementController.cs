using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Vector2 _boxSize;
    [SerializeField] private float _castDistance;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _coyoteTime;
    [SerializeField] private float _fallMultiplier;
    [SerializeField] private float _lowJumpMultiplier;
    [SerializeField] private float _maxFallSpeed;
    [SerializeField] private float _jumpApexSpeedMult;
    [SerializeField] private float _slowDownSpeed;
    private float _coyoteTimer = 0;
    private float _jumpCooldownTimer = 0;
    private float _jumpHoldTimer = 0;
    private bool _isJumpHeld = false;

    //Facing Direction is for the last direction looked at on horizontal axis
    public bool FacingDirection { get; private set; }
    //Look direction is for whether player is holding up, down or neither, 0 is neutral, 1 is up, -1 is down
    public int LookDirection { get; private set; }

    private float _moveX;
    private float _moveY;
    public bool CanMove { get { return _canMove; } }
    private bool _canMove = true;

    private Rigidbody2D _rb;
    private Animator _anim;

    private float spriteScaleX = 1;
    [SerializeField] private float squashInterpolation;

    private void Start()
    {
        _anim = transform.GetChild(0).GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        FacingDirection = true;
    }

    public void SetCanMove(bool move)
    {
        _canMove = move;
    }

    private void FixedUpdate()
    {

        //CoyoteTime
        if (_coyoteTimer > 0)
            _coyoteTimer -= Time.deltaTime;
        if (isGrounded())
            _coyoteTimer = _coyoteTime;
        if (_jumpCooldownTimer > 0)
            _jumpCooldownTimer -= Time.deltaTime;

        if (_jumpHoldTimer > 0)
        {
            _jumpHoldTimer -= Time.deltaTime;
            ApplyJump();
        }

        //Move
        if (_canMove && Mathf.Abs(_rb.velocity.x) < GameManager.Instance.PlayerStatsManager.MovementSpeed)
            _rb.velocity = new Vector2(_moveX * GameManager.Instance.PlayerStatsManager.MovementSpeed, _rb.velocity.y);

        //Better Jump
        if (_rb.velocity.y < 0)
        {
            //Increase downward speed
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
            if (_rb.velocity.y < -_maxFallSpeed)
                _rb.velocity = new Vector2(_rb.velocity.x, -_maxFallSpeed);
        }
        else if (_rb.velocity.y > 0 && !_isJumpHeld)
        {
            //Increase upward speed when jump is held
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
        }
        else if (_rb.velocity.y > .1f && _rb.velocity.y < 10 && _isJumpHeld && _canMove)
        {
            _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, _rb.velocity.x * _jumpApexSpeedMult, 5 * Time.deltaTime), _rb.velocity.y);
        }

        //Slowdown
        _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, _moveX * GameManager.Instance.PlayerStatsManager.MovementSpeed, Time.deltaTime * _slowDownSpeed), _rb.velocity.y);

        //Look Direction
        if (_moveY > 0)
            LookDirection = 1;
        else
        if (_moveY < 0)
            LookDirection = -1;
        else
            LookDirection = 0;


        //Movement Animation Code
        if (_moveX != 0)
            _anim.SetBool("Running", true);
        else
            _anim.SetBool("Running", false);
        if (!isGrounded())
            _anim.SetBool("Jump", true);
        else
            _anim.SetBool("Jump", false);

        if (FacingDirection)
            _anim.transform.localScale = new Vector3(spriteScaleX, 1, 1);
        else
            _anim.transform.localScale = new Vector3(-spriteScaleX, 1, 1);

        if (Mathf.Abs(_anim.transform.localScale.x) != 1)
        {
            spriteScaleX = Mathf.Lerp(spriteScaleX, 1, squashInterpolation * Time.deltaTime);
        }
    }

    public void SetInputs(Vector2 direction)
    {
        if (direction.x < 0)
        {
            _moveX = -1;
            FacingDirection = false;
        }
        else
        if (direction.x > 0)
        {
            _moveX = 1;
            FacingDirection = true;
        }
        else
            _moveX = 0;

        _moveY = direction.y;
    }

    public void Jump()
    {
        _jumpHoldTimer = .1f;
    }

    public void SetJumpButton(bool jumpHeld)
    {
        _isJumpHeld = jumpHeld;
    }

    private void ApplyJump()
    {
        if (_jumpCooldownTimer > 0)
            return;

        if (isGrounded() || _coyoteTimer > 0)
        {
            SpriteScale();
            VolatileSound.Create(GameManager.Instance.AssetManager.JumpSound);
            ForceJump(1);
        }
    }

    public void SpriteScale()
    {
        spriteScaleX = .55f;
    }

    public void ForceJump(float mult)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, GameManager.Instance.PlayerStatsManager.JumpHeight * mult);
        _coyoteTimer = 0;
        _jumpCooldownTimer = _coyoteTime;
        _jumpHoldTimer = 0;
    }

    public bool isGrounded()
    {
        if (Physics2D.BoxCast(transform.position, _boxSize, 0, -transform.up, _castDistance, _groundLayer))
            return true;
        else
            return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * _castDistance, _boxSize);
    }

    public void RecoilNudge(Vector2 direction)
    {
        Vector2 velocity = direction * GameManager.Instance.PlayerStatsManager.RecoilSpeed;
        _rb.velocity = velocity;
    }
}
