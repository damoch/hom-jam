
using Assets.Scripts;
using System.Collections;
#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine;

public class EnemyController : Character
{
    public GameObject DeathAnimation;
    public bool Stationary;
    public GameObject target;
    public float speed;
    public bool IgnoreCollisionsWithPlayer;
    private float speedTemp;
    private bool enemyInRange;
    private GameManager _gameManager;
    private AutoWeaponComponent _weapon;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _weapon = GetComponent<AutoWeaponComponent>();
        enemyInRange = false;
        speedTemp = speed;

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        if (IgnoreCollisionsWithPlayer)
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(),
                target.gameObject.GetComponent<BoxCollider2D>());
        }

        if(BaseObject == null)
        {
            BaseObject = gameObject;
        }

    }

    void Update()
    {
        if (!_weapon.CanShoot)
        {
            _weapon.CheckCanShoot();
        }
        _weapon.CheckFire(enemyInRange);
        FaceEnemy();
        if (Stationary)
        {
            return;
        }
        Vector3 dir = Vector2.up;
        dir *= speed * Time.deltaTime;
        gameObject.transform.Translate(dir);
    }

    private void FaceEnemy()
    {
        var dir = target.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg -90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
        }
    }

    public override void UpdateHealthValue(int hitpoints)
    {
        HealthPoints += hitpoints;
        if (HealthPoints < 0)
        {
            if(DeathAnimation != null)
            {
                Instantiate(DeathAnimation, transform.position, transform.rotation);
            }
            _gameManager.NotifyEnemyDestroyed(this);

        }
    }

    internal void OnDestruction()
    {
        Destroy(BaseObject);
    }
}
