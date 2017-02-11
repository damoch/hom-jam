using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public float speed;

    // Use this for initialization
    void Start()
    {        
        rigidbody = GetComponent<Rigidbody2D>();
        //speed = 800;
        //Debug.Log(speed);
        Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Destroy(gameObject);
            //Shoot();
        }
    }

    void Shoot()
    {
        Vector2 mouseDirect = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        rigidbody.AddForce(mouseDirect.normalized * speed);
        rigidbody.velocity = new Vector2(rigidbody.velocity.x * speed,
            rigidbody.velocity.y * speed);
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        Destroy(gameObject);
    }
}
