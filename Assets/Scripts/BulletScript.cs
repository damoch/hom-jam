using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public float speed;
    public int hitPoints;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    void Start()
    {        
        rigidbody = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(9,10);
        Shoot();
    }
    
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Destroy(gameObject);
        }
    }

    void Shoot()
    {
        rigidbody.AddForce(Vector2.up * speed);
        rigidbody.velocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.tag.Equals("Player"))
        {
            gameManager.DecreasePlayerHealth(hitPoints);
            Destroy(gameObject);
        }

        if (Other.tag.Equals("Boundary"))
        {
            Destroy(gameObject);
        }
    }
}
