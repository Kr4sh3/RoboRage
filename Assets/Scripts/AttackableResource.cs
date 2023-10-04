using UnityEngine;
using System.Collections.Generic;

public interface IDamageable
{
    public void Damage(int damage);
    public void DamageNonLethal(int damage);
}

public class AttackableResource : MonoBehaviour, IDamageable
{
    [SerializeField] private LootTable _droppedResources;
    [SerializeField] private float _healTime;

    private float _iFrameLength = .1f;
    private float _iFrameTimer;
    private float _healTimer;
    private float _dropVelocityStrength = 3;

    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;
    private HealthController _healthController;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _healthController = GetComponent<HealthController>();

        ResetHealTimer();
    }

    private void Update()
    {
        if (_iFrameTimer > 0)
            _iFrameTimer -= Time.deltaTime;

        //Return to default shader near end of iframes
        if (_iFrameTimer < 0.02f && _spriteRenderer.material.shader != AssetManager.Instance.DefaultSpriteShader)
            _spriteRenderer.material.shader = AssetManager.Instance.DefaultSpriteShader;

        //Count down to next heal if damaged
        if (_healthController.GetHealth() < _healthController.GetMaxHealth())
            _healTimer -= Time.deltaTime;

        if (_healTimer <= 0)
        {
            _healthController.Heal(1);
            ResetHealTimer();
        }
    }

    public void GiveIFrames()
    {
        StartIFrames();
    }

    private GameObject spawnSource;

    public void SetSpawnSource(GameObject source)
    {
        spawnSource = source;
    }

    public void Damage(int damage)
    {
        if (_iFrameTimer > 0)
            return;

        ResetHealTimer();
        StartIFrames();
        _healthController.Damage(damage);
        _spriteRenderer.material.shader = AssetManager.Instance.WhiteFlashShader;
        PlayDamageSound();

        if (_healthController.GetHealth() > 0)
            return;

        if (spawnSource != null)
            spawnSource.GetComponent<EnemySpawner>().RemoveFromArray(gameObject);
        DestroySelf();
    }

    public void DamageNonLethal(int damage)
    {
        if (_iFrameTimer > 0)
            return;
        ResetHealTimer();
        StartIFrames();
        _healthController.DamageNonLethal(damage);
        _spriteRenderer.material.shader = AssetManager.Instance.WhiteFlashShader;
        PlayDamageSound();
    }

    private void DropItems()
    {
        if (_droppedResources == null)
        {
            Debug.LogError(gameObject.name + " has no loottable");
            return;
        }
        bool itemDropped = false;
        while (!itemDropped)
        {
            foreach (KeyValuePair<ItemStack, float> o in _droppedResources.Items)
            {
                float chance = Random.Range(0f, 1f);
                if (chance < o.Value && !itemDropped)
                {
                    itemDropped = true;
                    GameObject item = CollectableItem.Create(o.Key);
                    item.transform.position = transform.position;
                    Vector2 randomDirection = new Vector2(Random.Range(-1000, 1000), Random.Range(-1000, 1000)).normalized;
                    item.GetComponent<Rigidbody2D>().velocity = randomDirection * _dropVelocityStrength;
                }
            }
        }
    }

    private void DestroySelf()
    {
        VolatileSound.Create(AssetManager.Instance.ResourceDestroyedSound);
        DropItems();
        Destroy(gameObject);
    }

    private void PlayDamageSound()
    {
        VolatileSound.Create(AssetManager.Instance.ResourceHitSound);
    }

    private void StartIFrames() { _iFrameTimer = _iFrameLength; }

    private void ResetHealTimer() { _healTimer = _healTime; }
}
