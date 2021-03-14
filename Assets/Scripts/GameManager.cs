using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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

    [Range(1, 100)]
    public int StartPlayerHealth;

    [Range(1, 100)]
    public int StartPlayerEnergy;

    public GameObject EnemyPrefab;
    public GameObject HealthPackPrefab;
    public string LoseSceneName;
    public GameState GameState;
    public KeyCode StartNewGameKey;
    public Vector2 PlayersStartPosition;
    public GameObject TitleScreen;

    private GameObject[] _spawnPoints;
    private PlayerControler _player;
    private LevelManager _levelManager;
    private float _elapsedFreezeFrames;
    private bool _inFreezeFrame;
    private int _enemyCount;
    private int _medkitsCount;
    private List<EnemyController> _enemiesOnMap;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerControler>();
        _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        _levelManager = FindObjectOfType<LevelManager>();
        _enemiesOnMap = new List<EnemyController>();
    }

    private void Start()
    {
        _medkitsCount = 0;
        _enemyCount = 0;
        _elapsedFreezeFrames = 0;
        _inFreezeFrame = false; 
        GameOver();
    }

    private void Update()
    {
        if(GameState == GameState.GameOn)
        {
            UpdateGamePlay();
        }
        else if(GameState == GameState.GameOver)
        {
            if (Input.GetKey(StartNewGameKey))
            {
                StartNewGame();
            }
        }

    }

    private void UpdateGamePlay()
    {
        if (_enemyCount < MaxEnemies)
        {
            _enemyCount++;
            _enemiesOnMap.Add(SpawnObjectsRandomly(EnemyPrefab, ChanceToSpawnEnemy).GetComponent<EnemyController>());
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

    private void StartNewGame()
    {
        TitleScreen.SetActive(false);
        GameState = GameState.GameOn;
        _player.SetStartGameValues(StartPlayerHealth);
        _player.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        TitleScreen.SetActive(true);
        GameState = GameState.GameOver;
        while(_enemiesOnMap.Count > 0)
        {
            NotifyEnemyDestroyed(_enemiesOnMap.First());
        }
        _player.transform.position = PlayersStartPosition;
        _player.gameObject.SetActive(false);
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
        _enemiesOnMap.Remove(enemy);
        Destroy(enemy.gameObject);
        _enemyCount--;
    }
}
