using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Range(0f, 100f)]
    public float FreezeTimeInFrames;

    [Range(0f, 1f)]
    public float ChanceToSpawnHealthPack;

    [Range(0f, 1f)]
    public float ChanceToSpawnEnemy;

    [Range(0f, 0.1f)]
    public float ChanceFactor;

    [Range(1, 100)]
    public int MaxEnemies;

    [Range(1, 100)]
    public int MaxHealthPacks;

    public GameObject EnemyPrefab;
    public GameObject HealthPackPrefab;
    public string LoseSceneName;

    private GameObject[] _spawnPoints;
    private PlayerControler _player;
    private LevelManager _levelManager;
    private float _elapsedFreezeFrames;
    private bool _inFreezeFrame;
    private int _enemyCount;
    private int _medkitsCount;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerControler>();
        _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        _levelManager = FindObjectOfType<LevelManager>();
    }

    private void Start()
    {
        _medkitsCount = 0;
        _enemyCount = 0;
        _elapsedFreezeFrames = 0;
        _inFreezeFrame = false;
    }

    private void Update()
    {
        if (_enemyCount < MaxEnemies)
        {
            _enemyCount++;
            SpawnObjectsRandomly(EnemyPrefab, ChanceToSpawnEnemy);
        }
        if (_medkitsCount < MaxHealthPacks)
        {
            _medkitsCount++;
            SpawnObjectsRandomly(HealthPackPrefab, ChanceToSpawnHealthPack).GetComponent<HealthPack>().MoveToPlayer(_player);
        }
        if (_inFreezeFrame)
        {
            _elapsedFreezeFrames++;

            if (_elapsedFreezeFrames >= FreezeTimeInFrames)
            {
                _inFreezeFrame = false;
                _elapsedFreezeFrames = 0;
                Time.timeScale = 1;
            }
        }
    }

    public void GameOver()
    {
        _levelManager.LoadLevel(LoseSceneName);
    }

    public void FreezeFrame()
    {
        if (_inFreezeFrame)
        {
            return;
        }
        _inFreezeFrame = true;
        Time.timeScale = 0;
    }

    internal void HealthKitDestroyed()
    {
        _medkitsCount--;
    }

    private GameObject SpawnObjectsRandomly(GameObject prefab, float chanceToSpawn)
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(_spawnPoints.Length);
        Vector3 position = _spawnPoints[index].transform.position;
        return Instantiate(prefab, position, Quaternion.identity);
    }

    internal void NotifyEnemyDestroyed(EnemyController enemy)
    {
        //FreezeFrame();
        Destroy(enemy.gameObject);
        _enemyCount--;
    }
}
