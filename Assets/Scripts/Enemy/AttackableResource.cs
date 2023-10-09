using UnityEngine;
using System.Collections.Generic;
using System;

[Obsolete]
public class AttackableResource : HealthController
{
    [SerializeField] private float _healTime;
    private float _healTimer;

    private SpriteRenderer _spriteRenderer;

    protected override void Start()
    {
        base.Start();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        ResetHealTimer();
    }

    protected override void Update()
    {
        base.Update();
        if (_iFrameTimer > 0)
            _iFrameTimer -= Time.deltaTime;

        //Return to default shader near end of iframes
        if (_iFrameTimer < 0.02f && _spriteRenderer.material.shader != GameManager.Instance.AssetManager.DefaultSpriteShader)
            _spriteRenderer.material.shader = GameManager.Instance.AssetManager.DefaultSpriteShader;

        //Count down to next heal if damaged
        if (Health < MaxHealth)
            _healTimer -= Time.deltaTime;

        if (_healTimer <= 0)
        {
            Heal(1);
            ResetHealTimer();
        }
    }

    private GameObject spawnSource;

    public void SetSpawnSource(GameObject source)
    {
        spawnSource = source;
    }

    public override bool Damage(int damage)
    {
        if (!base.Damage(damage))
            return false;

        ResetHealTimer();
        _spriteRenderer.material.shader = GameManager.Instance.AssetManager.WhiteFlashShader;
        VolatileSound.Create(GameManager.Instance.AssetManager.ResourceHitSound);
        return true;
    }

    public override void DamageNonLethal(int damage)
    {
        base.DamageNonLethal(damage);
    }

    public override void Die()
    {
        base.Die();
        if (spawnSource != null)
            spawnSource.GetComponent<EnemySpawner>().RemoveFromArray(gameObject);
        VolatileSound.Create(GameManager.Instance.AssetManager.ResourceDestroyedSound);
        Destroy(gameObject);
    }

    private void ResetHealTimer() { _healTimer = _healTime; }
}
