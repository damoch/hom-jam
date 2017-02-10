using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {
    static GameManager gameManager = null;

    public Player player;

    private void Start()
    {
        if(gameManager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            gameManager = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    public void IncreasePlayerHealth(int num)
    {
        player.health += num;
    }

    public static void DecreasePlayerHealth(int num)
    {
        player.health -= num;
    }

    public void LookAtPlayer(GameObject obj)
    {
        float angle = Mathf.Atan2((player.transform.position.y - obj.transform.position.y), (player.transform.position.x - obj.transform.position.x)) * Mathf.Rad2Deg;
        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void MoveToPlayer(GameObject obj, float force)
    {
        this.LookAtPlayer(obj);
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(new Vector2(1f*force, 0));
    }
}
