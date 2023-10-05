using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDamageable
{
    public bool CanCraft { get { return _canCraft; } set { _canCraft = value; } }
    [SerializeField] private bool _canCraft = false;
    private HealthController _healthController;
    private SideScrollerMovementController _movementController;
    private SpriteRenderer _spriteRenderer;
    public Transform lastCheckpoint;
    private Animator _anim;

    private float _iFrameLength = .75f;
    private float _iFrameTimer = 0;

    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _healthController = GetComponent<HealthController>();
        _movementController = GetComponent<SideScrollerMovementController>();
        _canCraft = false;
    }

    private void Update()
    {
        if (transform.position.y < -50f)
        {
            Damage(1);
        }

        if (_iFrameTimer > 0)
            _iFrameTimer -= Time.deltaTime;

        if (!_movementController.CanMove && _iFrameTimer > 0 && _iFrameTimer < .4f)
            _movementController.SetCanMove(true);

        //Return to default shader near end of iframes and allow player to move again
        if (_iFrameTimer < 0.02f && _spriteRenderer.material.shader != AssetManager.Instance.DefaultSpriteShader)
        {
            _spriteRenderer.material.shader = AssetManager.Instance.DefaultSpriteShader;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spikes"))
        {
            Damage(5);
        }
    }

    public void Damage(int damage)
    {
        if (_iFrameTimer > 0)
            return;

        StartIFrames();
        _spriteRenderer.material.shader = AssetManager.Instance.WhiteFlashShader;
        PlayDamageSound();
        _movementController.SetCanMove(false);

        _healthController.Damage(damage);

        _anim.SetTrigger("Hurt");

        GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().UpdateHealth(_healthController.GetHealth(), _healthController.GetMaxHealth());

        if (_healthController.GetHealth() <= 0)
        {
            //Die
            _healthController.Heal(5);
            GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().UpdateHealth(_healthController.GetHealth(), _healthController.GetMaxHealth());
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<InventorySystem>().DumpInventory();
            transform.position = lastCheckpoint.position;
            VolatileSound.Create(AssetManager.Instance.ResourceDestroyedSound);
        }
    }
    public void DamageNonLethal(int damage)
    {
        Debug.Log("Player should not be taking non-lethal damage!");
    }

    public void Heal(int amount)
    {
        _healthController.Heal(amount);
        GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().UpdateHealth(_healthController.GetHealth(), _healthController.GetMaxHealth());
        VolatileSound.Create(AssetManager.Instance.Heal);
    }

    private void PlayDamageSound()
    {
        VolatileSound.Create(AssetManager.Instance.PlayerDamage);
    }

    private void StartIFrames() { _iFrameTimer = _iFrameLength; }

    public void SetLastCheckpoint(Transform checkpoint)
    {
        lastCheckpoint = checkpoint;
    }
}
