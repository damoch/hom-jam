using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : Character
{
    public KeyCode LEFT;
    public KeyCode RIGHT;
    public KeyCode UP;
    public KeyCode DOWN;
    [Range(0, 100)]
    public float Speed;
    [Range(0, 100)]
    public int MaxHealthPoints;
    [Range(0, 100)]
    public int HighHealth;
    [Range(0, 100)]
    public int LowHealth;
    [Range(0, 100)]
    public int MinHealthPoints;
    [Range(0, 10)]
    public float ShootCooldown;
    [Range(0, 200)]
    public float MaxWeaponHeat;
    [Range(0, 200)]
    public float MinWeaponHeat;
    [Range(0, 200)]
    public float WeaponHeatEveryShot;
    [Range(0, 200)]
    public float WeaponHeatDrop;

    public GameObject BulletPrefab;
    public GameObject BulletSpawnPoint;

    private float _moveX;
    private float _moveY;
    private Rigidbody2D _rigidboy;
    private Vector2 _mousePosition;
    private bool _canShoot;

    private float _animationSpeed;
    public Text HealthText;
    private Animator _animator;

    private float _elapsedCooldown;
    private float _currentWeaponHeat;
    private bool _weaponOverheat;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidboy = GetComponent<Rigidbody2D>();
        _canShoot = true;
        _currentWeaponHeat = MinWeaponHeat;

        _animationSpeed = _animator.speed;
        _animator.speed = 0f;
        _elapsedCooldown = 0f;
        _weaponOverheat = false;
    }

    private void Update()
    {
        if (!_canShoot)
        {
            CheckCanShoot();
        }
        CheckFire();
        PlayerMoving();
        ChangePlayerLookingDirection();
        UpdatePlayerLife();
    }

    private void CheckCanShoot()
    {
        _elapsedCooldown += Time.deltaTime;

        if (_elapsedCooldown >= ShootCooldown)
        {
            _canShoot = true;
            _elapsedCooldown = 0f;
        }
    }

    private void UpdatePlayerLife()
    {
        HealthText.text = HealthPoints.ToString();
    }

    private void PlayerMoving()
    {
        _moveX = Input.GetAxis("Horizontal");
        _moveY = Input.GetAxis("Vertical");

        if (_moveX != 0 || _moveY != 0)
        {
            _animator.speed = _animationSpeed;
        }

        else
        {
            _animator.speed = 0f;
        }

        _rigidboy.velocity = new Vector2(_moveX * Speed, _moveY * Speed);
    }

    private void CheckFire()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }   
        else if(_currentWeaponHeat > MinWeaponHeat)
        {
            _currentWeaponHeat -= WeaponHeatDrop;
            if(_currentWeaponHeat <= MinWeaponHeat)
            {
                _weaponOverheat = false;
            }
        }
    }

    private void ChangePlayerLookingDirection()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (_mousePosition - (Vector2)transform.position).normalized;
        transform.up = direction;
    }

    private void Shoot()
    {
        if (!_canShoot || _weaponOverheat)
        {
            return;
        }
        Instantiate(BulletPrefab, BulletSpawnPoint.transform.position, transform.rotation);
        _canShoot = false;

        _currentWeaponHeat += WeaponHeatEveryShot;
        if(_currentWeaponHeat > MaxWeaponHeat)
        {
            _weaponOverheat = true;
        }
    }

    public override void UpdateHealthValue(int hitpoints)
    {
        HealthPoints += hitpoints;
        if (HealthPoints > MaxHealthPoints || HealthPoints < MinHealthPoints)
        {
            FindObjectOfType<GameManager>().ResetScene();

        }
        UpdatePlayerLife();
    }
}

