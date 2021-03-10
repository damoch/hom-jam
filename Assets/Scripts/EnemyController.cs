
using Assets.Scripts;
using System.Collections;
#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine;

public class EnemyController : Character
{
    public GameObject Bullet;
    public Transform target;
    public float speed;
    public float ShootingTimeMin;
    public float ShootingTimeMax;
    public float nextWaypointDistance; 
    public float newPathSpan;

    private float speedTemp;
    private bool enemyInRange;
    private GameManager _gameManager;
    private Vector3 _currentTarget;
    private AutoWeaponComponent _weapon;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _weapon = GetComponent<AutoWeaponComponent>();
        enemyInRange = false;
        speedTemp = speed;

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        GoToPoint(target.transform.position);
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(),
            target.gameObject.GetComponent<BoxCollider2D>());
        
    }

    void Update()
    {
        if (!_weapon.CanShoot)
        {
            _weapon.CheckCanShoot();
        }
        _weapon.CheckFire(enemyInRange);
        FaceEnemy();
        Vector3 dir = (_currentTarget - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;
        gameObject.transform.Translate(dir);
        Debug.Log(Vector3.Distance(_currentTarget, target.transform.position));
        if (Vector3.Distance(_currentTarget, target.transform.position) < nextWaypointDistance)
        {
            GoToPoint(target.transform.position);
            return;
        }
    }

    void FaceEnemy()
    {
        var dir = target.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg -90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void GoToPoint(Vector2 point)
    {
        _currentTarget = point;
        Debug.Log(point);
    }

    public void TriggerEnter(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            enemyInRange = true;
            speed = 0;
        }
    }

    public void TriggerExit(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            enemyInRange = false;
            speed = speedTemp;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            GoToPoint(target.transform.position);
        }
    }

    public override void UpdateHealthValue(int hitpoints)
    {
        HealthPoints += hitpoints;
        if (HealthPoints < 0)
        {
            //_gameManager.FreezeFrame();
            _gameManager.NotifyEnemyDestroyed(this);
            //Destroy(gameObject);

        }
    }
}
