using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;


public class HealthController : MonoBehaviour
{

    public int MaxHealth
    {
        get { return _maxHealth; }
        set
        {
            if (value <= 0)
                return;
            _maxHealth = value;
            if (_health > _maxHealth)
                _health = _maxHealth;
        }
    }
    [SerializeField] private int _maxHealth;

    public int Health
    {
        get { return _health; }
        private set
        {
            _health = value;
            if (value <= 0)
                _health = 0;
            if (_health > _maxHealth)
                _health = _maxHealth;
        }
    }
    private int _health;

    public float IFrameLength { get { return _iFrameLength; } set { if (value >= 0) _iFrameLength = value; } }
    protected float _iFrameLength = .075f;
    protected float _iFrameTimer = 0;

    protected virtual void Start() { Health = MaxHealth; }

    protected virtual void Update()
    {
        if (_iFrameTimer > 0)
            _iFrameTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Deals damage to the player, 
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>
    /// Returns true when damage was successfully dealt
    /// </returns>
    public virtual bool Damage(int damage)
    {
        if (_iFrameTimer > 0 || IsDead())
            return false;

        Health -= damage;
        if (!IsDead())
            _iFrameTimer = IFrameLength;
        else
            Die();

        return true;
    }

    /// <summary>
    /// Alternative overrideable damage method that will not kill the object.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void DamageNonLethal(int damage)
    {
        int nonlethalDamage = Health - 1;
        if (damage > nonlethalDamage)
            Damage(nonlethalDamage);
        else
            Damage(damage);
    }

    /// <summary>
    /// Kills the object with health (Must be overridden to handle death)
    /// </summary>
    public virtual void Die()
    {
        if (Health > 0)
            Health = 0;
        _iFrameTimer = 0;
    }

    public virtual void Heal(int amount)
    {
        if (!IsAtFullHealth())
            Health += amount;
    }

    public bool IsDead()
    {
        if (Health == 0)
            return true;
        else
            return false;
    }

    public bool IsAtFullHealth()
    {
        if (Health == MaxHealth)
            return true;
        else
            return false;
    }
}
