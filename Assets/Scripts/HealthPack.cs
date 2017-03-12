using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

    public int points;
    public float force;

    private GameManager gameManager;
    private Vector3 vectorToPlayer;

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        gameManager.MoveToPlayer(gameObject, force);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //gameManager.IncreasePlayerHealth(points);
            collision.gameObject.GetComponent<Character>().UpdateHealthValue(points);
            Destroy(gameObject);
        }
        else if (collision.tag == "Boundary")
        {
            Destroy(gameObject);
        }
    }
}
