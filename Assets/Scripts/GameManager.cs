using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {
    static GameManager gameManager = null;

    [Range(0f, 1f)]
    public float chanceToSpawnHealthPack;

    [Range(0f, 1f)]
    public float chanceToSpawnEnemy;

    [Range(0f, 0.1f)]
    public float chanceFactor;

    [Range(1, 100)]
    public int maxEnemies;

    [Range(1, 100)]
    public int maxHealthPacks;

    public GameObject enemyPrefab;
    public GameObject healthPackPrefab;

    private GameObject[] spawnPoints;
    private PlayerControler player;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerControler>();
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

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

    private void Update()
    {
        if(GameObject.FindGameObjectsWithTag(enemyPrefab.tag).Length < maxEnemies)
        {
            SpawnObjectsRandomly(enemyPrefab, chanceToSpawnEnemy);
        }
        if(GameObject.FindGameObjectsWithTag(healthPackPrefab.tag).Length < maxHealthPacks)
        {
            SpawnObjectsRandomly(healthPackPrefab, chanceToSpawnHealthPack);
        }
    }

    public void IncreasePlayerHealth(int num)
    {
        player.healthPoints += num;
    }

    public void DecreasePlayerHealth(int num)
    {
        player.healthPoints -= num;
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

    private void SpawnObjectsRandomly(GameObject prefab, float chanceToSpawn)
    {
        System.Random rnd = new System.Random();

        if (rnd.NextDouble() > chanceToSpawn*chanceFactor)
            return;
        else
        {
            int numOfObjects = rnd.Next(1, spawnPoints.Length);

            for (int i = 0; i < numOfObjects; i++)
            {
                int index = rnd.Next(spawnPoints.Length);
                Vector3 position = spawnPoints[index].transform.position;
                GameObject obj = Instantiate(prefab, position, Quaternion.identity);
            }
        }
    }
}
