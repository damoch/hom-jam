using Assets.Scripts;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : Character
{
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

    public GameObject BulletPrefab;
    public GameObject BulletSpawnPoint;
    public GameManager GameManager;

    private float _moveX;
    private float _moveY;
    private Rigidbody2D _rigidboy;
    private Vector2 _mousePosition;

    private float _animationSpeed;
    public Slider HealthSlider;
    private Animator _animator;
    private AutoWeaponComponent _autoWeapon;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidboy = GetComponent<Rigidbody2D>();

        _animationSpeed = _animator.speed;
        _animator.speed = 0f;
        _autoWeapon = GetComponent<AutoWeaponComponent>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        HealthSlider.maxValue = MaxHealthPoints;
        HealthSlider.minValue = MinHealthPoints;
    }

    private void Update()
    {
        if(GameManager.GameState == Assets.Scripts.Enums.GameState.GameOverSlowdown)
        {
            return;
        }
        if (!_autoWeapon.CanShoot)
        {
            _autoWeapon.CheckCanShoot();
        }
        _autoWeapon.CheckFire(Input.GetMouseButton(0));
        PlayerMoving();
        ChangePlayerLookingDirection();
        UpdatePlayerLife();
    }

    private void UpdatePlayerLife()
    {
        HealthSlider.value = HealthPoints;
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

    private void ChangePlayerLookingDirection()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (_mousePosition - (Vector2)transform.position).normalized;
        transform.up = direction;
    }

    public override void UpdateHealthValue(int hitpoints)
    {
        if(GameManager.GameState != GameState.GameOn)
        {
            return;
        }
        if (hitpoints > 0 && HealthPoints < MaxHealthPoints && HealthPoints + hitpoints > MaxHealthPoints)
        {
            HealthPoints = MaxHealthPoints;
            UpdatePlayerLife();
            return;
        }
        HealthPoints += hitpoints;
        if (HealthPoints > MaxHealthPoints || HealthPoints < MinHealthPoints)
        {
            _autoWeapon.ResetWeapon();
            _spriteRenderer.enabled = false;
            GameManager.BeginGameOver();
        }
        UpdatePlayerLife();
    }

    internal void SetStartGameValues(int startPlayerHealth)
    {
        HealthPoints = startPlayerHealth;
        UpdatePlayerLife();
        _spriteRenderer.enabled = true;
    }
}

