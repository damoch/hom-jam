using Assets.Scripts;
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

    public GameObject BulletPrefab;
    public GameObject BulletSpawnPoint;

    private float _moveX;
    private float _moveY;
    private Rigidbody2D _rigidboy;
    private Vector2 _mousePosition;

    private float _animationSpeed;
    public Text HealthText;
    private Animator _animator;
    private AutoWeaponComponent _autoWeapon;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidboy = GetComponent<Rigidbody2D>();

        _animationSpeed = _animator.speed;
        _animator.speed = 0f;
        _autoWeapon = GetComponent<AutoWeaponComponent>();
    }

    private void Update()
    {
        if (!_autoWeapon.CanShoot)
        {
            _autoWeapon.CheckCanShoot();
        }
        _autoWeapon.CheckFire();
        PlayerMoving();
        ChangePlayerLookingDirection();
        UpdatePlayerLife();
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

    private void ChangePlayerLookingDirection()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (_mousePosition - (Vector2)transform.position).normalized;
        transform.up = direction;
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

