using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : HealthController
{
    private SideScrollerMovementController _moveController;
    private SpriteRenderer _spriteRenderer;
    public Transform LastCheckpoint;
    private Animator _anim;
    [SerializeField] GameObject _bulletPrefab;

    private bool _attacking = false;
    private float _attackTimer = 0;

    protected override void Start()
    {
        base.Start();
        _anim = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _moveController = GetComponent<SideScrollerMovementController>();

        GameManager.Instance.InputManager.PlayerController = this;
    }

    protected override void Update()
    {
        base.Update();
        Shoot();

        //Death bounds
        if (transform.position.y < -50f)
        {
            Damage(1);
        }

        //Return to default shader near end of iframes and allow player to move again
        if (_iFrameTimer < 0.02f && _spriteRenderer.material.shader != GameManager.Instance.AssetManager.DefaultSpriteShader)
        {
            _spriteRenderer.material.shader = GameManager.Instance.AssetManager.DefaultSpriteShader;
            _moveController.SetCanMove(true);
        }
    }

    #region Controls
    public void AttackPerformed()
    {
        _attacking = true;
    }

    public void AttackCanceled()
    {
        _attacking = false;
    }

    public void Movement(Vector2 move)
    {
        _moveController.SetInputs(move);
    }

    public void Jump()
    {
        _moveController.Jump();
        _moveController.SetJumpButton(true);
    }

    public void CancelJump()
    {
        _moveController.SetJumpButton(false);
    }
    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spikes"))
        {
            Damage(5);
        }
    }

    public override bool Damage(int damage)
    {
        if (!base.Damage(damage))
            return false;

        _spriteRenderer.material.shader = GameManager.Instance.AssetManager.WhiteFlashShader;
        VolatileSound.Create(GameManager.Instance.AssetManager.PlayerDamage);
        _moveController.SetCanMove(false);
        _anim.SetTrigger("Hurt");
        GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().UpdateHealth(Health, MaxHealth);

        return true;
    }

    public override void Die()
    {
        base.Die();
        Heal(5);
        GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().UpdateHealth(Health, MaxHealth);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        transform.position = LastCheckpoint.position;
        VolatileSound.Create(GameManager.Instance.AssetManager.ResourceDestroyedSound);
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().UpdateHealth(Health, MaxHealth);
        VolatileSound.Create(GameManager.Instance.AssetManager.Heal);
    }

    public void SetLastCheckpoint(Transform checkpoint)
    {
        LastCheckpoint = checkpoint;
    }

    private void Shoot()
    {

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
        VolatileSound.Create(GameManager.Instance.AssetManager.ShootSound);
        _attackTimer = GameManager.Instance.PlayerStatsManager.AttackCooldown;
        bullet.bulletDamage = Mathf.RoundToInt(GameManager.Instance.PlayerStatsManager.BulletDamage);
        bullet.bulletSpeed = GameManager.Instance.PlayerStatsManager.BulletSpeed;
        bullet.bulletLife = GameManager.Instance.PlayerStatsManager.BulletLifeTime;
    }
}
