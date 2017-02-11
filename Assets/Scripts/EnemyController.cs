using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject Bullet;
    public Transform target;
    private Seeker seeker;  
    public Path path;   
    public float speed;
    public float ShootingTimeMin;
    public float ShootingTimeMax;
    public float nextWaypointDistance; 
    private int currentWaypoint;
    
    private float speedTemp;
    private bool canShoot;
    private bool enemyInRange;

    void Start()
    {
        enemyInRange = false;
        canShoot = true;
        currentWaypoint = 0;
        seeker = GetComponent<Seeker>();
        speedTemp = speed;

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        GoToPoint(target.transform.position);
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(),
            target.gameObject.GetComponent<BoxCollider2D>());
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Update()
    {
        if (enemyInRange && canShoot)
        {
            StartCoroutine("ShootBullet");
        }

        if (path == null)
        {
            //return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if(Vector2.Distance(transform.position,target.transform.position)> 1)
            GoToPoint(target.transform.position);
            return;
        }

        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;
        this.gameObject.transform.Translate(dir);

        if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
        
    }

    void FaceEnemy()
    {
        var dir = target.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void GoToPoint(Vector2 point)
    {
        seeker.StartPath(transform.position, point, OnPathComplete);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            enemyInRange = true;
            speed = 0;
            //StartCoroutine(shootingHandle);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            enemyInRange = false;
            speed = speedTemp;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            GoToPoint(target.transform.position);
        }
    }

    IEnumerator ShootBullet()
    {
        canShoot = false;
        FaceEnemy();
        Debug.Log("New bullet");
        Instantiate(Bullet, transform.position,transform.rotation);
        float timeSpan = UnityEngine.Random.Range(ShootingTimeMin, ShootingTimeMax);
        yield return new WaitForSeconds(timeSpan);
        canShoot = true;
    }
}
