using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public List<string> HitableTags;
    public List<string> DestroyOnHitTags;
    public List<string> PenatrableTags;
    public Rigidbody2D rigidbody;
    public float speed;
    public int hitPoints;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        HitableTags = HitableTags.ConvertAll(s => s.ToUpper());
        PenatrableTags = PenatrableTags.ConvertAll(s => s.ToUpper());
        DestroyOnHitTags = DestroyOnHitTags.ConvertAll(s => s.ToUpper());
    }

    void Start()
    {        
        rigidbody = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(9,10);
        Shoot();
    }
    
    void Update()
    {
    }

    void Shoot()
    {
        rigidbody.AddForce(Vector2.up * speed);
        rigidbody.velocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var tag = other.tag.ToUpper();

        if (HitableTags.Contains(tag))
        {
           other.gameObject.GetComponent<Character>().UpdateHealthValue(-hitPoints);
            Destroy(gameObject);
        }
        if (PenatrableTags.Contains(tag))
        {
            other.gameObject.GetComponent<Character>().UpdateHealthValue(-hitPoints);
        }
        else if(!tag.Equals("UNTAGGED"))
        {
            Destroy(gameObject);
        }
        //if (Other.tag.Equals("Player"))
        //{
        //    gameManager.DecreasePlayerHealth(hitPoints);
        //    Destroy(gameObject);
        //}

        //if (Other.tag.Equals("Boundary"))
        //{
        //    Destroy(gameObject);
        //}

        //if (Other.tag.Equals("Enemy"))
        //{
        //    EnemyController enemy =
        //    Other.gameObject.GetComponent<EnemyController>();
        //    enemy.Health -= hitPoints;

        //    if(enemy.Health<=0)
        //        Destroy(Other.gameObject);

        //    Destroy(gameObject);
        //}
    }
}
