using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : HealthController
{
    private MovementController _moveController;
    public GunController GunController { get; private set; }
    private SpriteRenderer _spriteRenderer;
    public Transform LastCheckpoint;
    private Animator _anim;
    private bool _attacking = false;

    protected override void Start()
    {
        base.Start();
        _iFrameLength = .2f;
        _anim = transform.GetChild(0).GetComponent<Animator>();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _moveController = GetComponent<MovementController>();
        GunController = GetComponentInChildren<GunController>();

        GameManager.Instance.InputManager.PlayerController = this;
    }

    protected override void Update()
    {
        base.Update();

        if (_attacking)
            GunController.Shoot();

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
        if (GunController != null)
            GunController.Aim(aim);
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Saw"))
        {
            Damage(1);
            GetComponent<Rigidbody2D>().velocity = (new Vector2(transform.position.x, transform.position.y) - other.ClosestPoint(transform.position)).normalized * 25;
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

        return true;
    }

    public override void Die()
    {
        Debug.Log("Death");
        /*
        base.Die();
        Heal(5);
        GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().UpdateHealth(Health, MaxHealth);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        transform.position = LastCheckpoint.position;
        VolatileSound.Create(GameManager.Instance.AssetManager.ResourceDestroyedSound);
        */
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
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
