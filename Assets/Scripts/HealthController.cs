using UnityEngine;
using Sirenix.OdinInspector;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    [ShowInInspector] [ReadOnly] private int _health;

    private void Start() { _health = _maxHealth; }

    public void Damage(int damage) { _health -= damage; }

    public void DamageNonLethal(int damage)
    {
        if (_health > 0)
            _health -= damage;
        if (_health <= 0)
            _health = 1;
    }

    public void Heal(int amount)
    {
        if (_health <= 0)
            _health = 0;
        if (_health < _maxHealth)
            _health += amount;
        if (_health > _maxHealth)
            _health = _maxHealth;
    }
    public int GetHealth() { return _health; }

    public int GetMaxHealth() { return _maxHealth; }

    public void AddMaxHealth() { _maxHealth++; }


}
