using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : HealthController
{
    private MovementController _moveController;
    private GunController _gunController;
    private SpriteRenderer _spriteRenderer;
    public Transform LastCheckpoint;
    private Animator _anim;
    private bool _attacking = false;

    protected override void Start()
    {
        base.Start();
        _anim = transform.GetChild(0).GetComponent<Animator>();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _moveController = GetComponent<MovementController>();
        _gunController = GetComponentInChildren<GunController>();

        GameManager.Instance.InputManager.PlayerController = this;
    }

    protected override void Update()
    {
        base.Update();

        if (_attacking)
            _gunController.Shoot();

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

    public void Aim(Vector2 aim)
    {
        if (_gunController != null)
            _gunController.Aim(aim);
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

    public void Recoil(Vector2 direction)
    {
        _moveController.RecoilNudge(direction);
    }
}
