using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GunInfo
{
    public GunInfo(bool reloading, int ammoCount, int maxAmmo, float reloadPercentage)
    {
        Reloading = reloading;
        AmmoCount = ammoCount;
        MaxAmmoCount = maxAmmo;
        ReloadPercentage = reloadPercentage;
    }
    public bool Reloading { get; }
    public int AmmoCount { get; }
    public int MaxAmmoCount { get; }
    public float ReloadPercentage { get; }
}

public class GunController : MonoBehaviour
{
    public LayerMask LayerMask;
    [SerializeField] private float _distanceFromPlayer;
    [SerializeField] private Vector2 _offset;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] private int _numberOfBullets;
    [SerializeField] private float _inaccuracy;
    [SerializeField] private int _maxAmmo;
    private bool _reloading = false;
    private int _ammoCount;
    private Vector2 _direction;
    private float _reloadTimer = 0;
    private float _attackTimer = 0;
    private float _resetRotationTimer = 0;

    public void Start()
    {
        _ammoCount = _maxAmmo;
    }

    public void Update()
    {
        if (_resetRotationTimer > 0)
            _resetRotationTimer -= Time.deltaTime;
        if (_attackTimer > 0)
            _attackTimer -= Time.deltaTime;
        if (_reloadTimer > 0)
            _reloadTimer -= Time.deltaTime;
        if (_reloadTimer <= 0 && _reloading)
        {
            Reload();
        }
        if (_ammoCount < _maxAmmo && !_reloading)
        {
            _reloadTimer = GameManager.Instance.PlayerStatsManager.ReloadTime;
            _reloading = true;
        }
    }

    public void Aim(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return;
        _direction = direction;
        //Position
        transform.localPosition = (direction * _distanceFromPlayer) + _offset;
        //Rotation
        float angle = AngleBetweenPoints(direction, Vector2.zero);
        Vector3 euler = new Vector3(0f, 0f, angle);
        transform.localRotation = Quaternion.Euler(euler);
        //Scale
        if (angle < -90 || angle > 90)
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);
        else
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
    }
    private float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
    public void Shoot()
    {
        if (_attackTimer > 0 || _ammoCount <= 0)
            return;
        _reloadTimer = GameManager.Instance.PlayerStatsManager.ReloadTime;
        _ammoCount--;
        for (int i = 0; i < _numberOfBullets; i++)
        {
            BulletController bullet;
            //Instantiate Bullet
            bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity).GetComponent<BulletController>();
            _attackTimer = GameManager.Instance.PlayerStatsManager.AttackCooldown;
            bullet.bulletDamage = Mathf.RoundToInt(GameManager.Instance.PlayerStatsManager.BulletDamage);
            bullet.bulletSpeed = GameManager.Instance.PlayerStatsManager.BulletSpeed;
            bullet.bulletLife = GameManager.Instance.PlayerStatsManager.BulletLifeTime;
            bullet.Direction = _direction;
            bullet.Rotation = transform.localRotation;
            bullet.LayerMask = LayerMask;
            bullet.inaccuracy = _inaccuracy;
        }
        VolatileSound.Create(GameManager.Instance.AssetManager.ShootSound);
        //Recoil
        transform.parent.GetComponent<PlayerController>().Recoil(-_direction);
    }
    public void Reload()
    {
        _reloading = false;
        _ammoCount = _maxAmmo;
        VolatileSound.Create(GameManager.Instance.AssetManager.ReloadSound);
    }
    public GunInfo GetGunInfo()
    {
        return new GunInfo(_reloading, _ammoCount, _maxAmmo, _reloadTimer / GameManager.Instance.PlayerStatsManager.ReloadTime);
    }
}
