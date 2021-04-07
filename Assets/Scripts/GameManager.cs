using Assets.Scripts;
using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

    [Range(0, 100)]
    public int MaxEnemies;

    [Range(0, 100)]
    public int MaxTowers;

    [Range(0, 100)]
    public int MaxHealthPacks;

    [Range(1, 100)]
    public int StartPlayerHealth;

    [Range(1, 100)]
    public int StartPlayerEnergy;

    [Range(0, 300)]
    public int GameOverSlowdownLengthFrames;

    [Range(0f, 1f)]
    public float GameOverSlowdownTimeScaleBegin;

    [Range(0f, 1f)]
    public float GameOverSlowdownTimeScaleEnd;

    public GameObject EnemyPrefab;
    public GameObject HealthPackPrefab;
    public GameObject EnemyTowerPrefab;
    public string LoseSceneName;
    public GameState GameState;
    public KeyCode StartNewGameKey;
    public Vector2 PlayersStartPosition;
    public GameObject TitleScreen;
    public GameObject GameScreen;
    public Text EnemiesDestroyedCounterText;
    public bool DisableSpawns;
    public GameObject PlayerBoom;
    public MusicController MusicController;
    public Text HiScoreText;
    public GameObject NewRecordText;

    private GameObject[] _spawnPoints;
    private PlayerControler _player;
    private float _elapsedFreezeFrames;
    private bool _inFreezeFrame;
    private int _enemyCount;
    private int _medkitsCount;
    private List<EnemyController> _enemiesOnMap;
    private int _enemiesDestroyed;
    private float _elapsedGameOverSlowdownFrames;
    private float _timeScaleDropEveryFrame;
    private int _towersCount;
    private int _hiScore;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerControler>();
        _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        _enemiesOnMap = new List<EnemyController>();
    }

    private void Start()
    {
        _towersCount = 0;
        _medkitsCount = 0;
        _enemyCount = 0;
        _elapsedFreezeFrames = 0;
        _inFreezeFrame = false;
        _timeScaleDropEveryFrame = (GameOverSlowdownTimeScaleBegin - GameOverSlowdownTimeScaleEnd) / GameOverSlowdownLengthFrames;

        if (PlayerPrefs.HasKey(PlayerPrefsKeys.HiScore.ToString()))
        {
            _hiScore = PlayerPrefs.GetInt(PlayerPrefsKeys.HiScore.ToString());
        }

        GameOver();
    }

    private void Update()
    {
        switch (GameState)
        {
            case GameState.GameOn:
                UpdateGamePlay();
                break;
            case GameState.GameOver:
                if (Input.GetKey(StartNewGameKey))
                {
                    StartNewGame();
                }
                break;
            case GameState.GameOverSlowdown:
                UpdateSlowdown();
                break;
        }

    }

    private void UpdateSlowdown()
    {
        _elapsedGameOverSlowdownFrames++;
        Time.timeScale -= _timeScaleDropEveryFrame;
        if (_elapsedGameOverSlowdownFrames >= GameOverSlowdownLengthFrames)
        {
            Time.timeScale = 1;
            _elapsedGameOverSlowdownFrames = 0;
            GameOver();
        }
    }

    private void UpdateGamePlay()
    {
        if (_enemyCount < MaxEnemies && !DisableSpawns)
        {
            _enemyCount++;
            var enemy = SpawnObjectsRandomly(EnemyPrefab, ChanceToSpawnEnemy).GetComponent<EnemyController>();
            enemy.SetTarget(_player.gameObject);
            _enemiesOnMap.Add(enemy);
        }
        if (_towersCount < MaxTowers && !DisableSpawns)
        {
            _towersCount++;
            var tower = SpawnObjectsRandomly(EnemyTowerPrefab, ChanceToSpawnEnemy).GetComponentInChildren<EnemyController>();
            tower.SetTarget(_player.gameObject);
            _enemiesOnMap.Add(tower);
        }
        if (_medkitsCount < MaxHealthPacks && !DisableSpawns)
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
        GameScreen.SetActive(true);
        GameState = GameState.GameOn;
        _player.SetStartGameValues(StartPlayerHealth);
        _player.gameObject.SetActive(true);
        MusicController.PlayInGameMusic();
        NewRecordText.SetActive(false);
    }

    public void BeginGameOver()
    {
        Time.timeScale = GameOverSlowdownTimeScaleBegin;
        Instantiate(PlayerBoom, _player.gameObject.transform.position, _player.gameObject.transform.rotation);
        GameState = GameState.GameOverSlowdown;
    }

    private void GameOver()
    {
        TitleScreen.SetActive(true);
        GameScreen.SetActive(false);
        GameState = GameState.GameOver;
        DestroyAllHomingProjectiles();
        while (_enemiesOnMap.Count > 0)
        {
            NotifyEnemyDestroyed(_enemiesOnMap.First());
        }
        _player.transform.position = PlayersStartPosition;
        _player.gameObject.SetActive(false);
        CheckHiScore();
        _enemiesDestroyed = 0;
        SetEnemiesDestroyedText();
        MusicController.PlayMenuMusic();
    }

    private void CheckHiScore()
    {
        if(_hiScore < _enemiesDestroyed)
        {
            _hiScore = _enemiesDestroyed;
            PlayerPrefs.SetInt(PlayerPrefsKeys.HiScore.ToString(), _hiScore);
        }
        HiScoreText.text = _hiScore.ToString();
    }

    private void DestroyAllHomingProjectiles()
    {
        var projectiles = FindObjectsOfType<HomingMissle>();

        foreach (var item in projectiles)
        {
            Destroy(item.gameObject);
        }
    }

    private void SetEnemiesDestroyedText()
    {
        EnemiesDestroyedCounterText.text = _enemiesDestroyed.ToString();
        if(_enemiesDestroyed> _hiScore)
        {
            NewRecordText.SetActive(true);
        }
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
        //TODO: More differences between enemies

        _enemiesDestroyed++;
        SetEnemiesDestroyedText();
        _enemiesOnMap.Remove(enemy);
        if (enemy.Stationary)
        {
            _towersCount--;
        }
        else
        {
            _enemyCount--;
        }
        enemy.OnDestruction();
    }
}
