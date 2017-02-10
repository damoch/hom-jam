﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

    public int points;
    public float force;

    GameManager gameManager;

    private Vector3 vectorToPlayer;

    private void Awake()
    {
        gameManager = GameManager.FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        gameManager.MoveToPlayer(gameObject, force);
    }

    private void Update()
    {
        Debug.Log(gameObject.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //GameManager.IncreasePlayerHealth(points);
            Destroy(gameObject);
        }
        else if (collision.tag == "Boundary")
        {
            Destroy(gameObject);
        }
    }
}