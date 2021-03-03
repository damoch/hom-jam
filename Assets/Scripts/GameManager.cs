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

    private void Awake()
    {
        _player = FindObjectOfType<PlayerControler>();
        _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        _levelManager = FindObjectOfType<LevelManager>();
    }

    private void Start()
    {
        _elapsedFreezeFrames = 0;
        _inFreezeFrame = false;
    }

    private void Update()
    {
        //TODO: Searching for objects twice on every frame.... Yeah, that smells
        if (GameObject.FindGameObjectsWithTag(EnemyPrefab.tag).Length < MaxEnemies)
        {
            SpawnObjectsRandomly(EnemyPrefab, ChanceToSpawnEnemy);
        }
        if (GameObject.FindGameObjectsWithTag(HealthPackPrefab.tag).Length < MaxHealthPacks)
        {
            SpawnObjectsRandomly(HealthPackPrefab, ChanceToSpawnHealthPack);
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

    public void ResetScene()
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

    public void LookAtPlayer(GameObject obj)
    {
        float angle = Mathf.Atan2((_player.transform.position.y - obj.transform.position.y), (_player.transform.position.x - obj.transform.position.x)) * Mathf.Rad2Deg;
        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void MoveToPlayer(GameObject obj, float force)
    {
        LookAtPlayer(obj);
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(new Vector2(1f * force, 0));
    }

    private void SpawnObjectsRandomly(GameObject prefab, float chanceToSpawn)
    {
        System.Random rnd = new System.Random();

        if (rnd.NextDouble() > chanceToSpawn * ChanceFactor)
            return;
        else
        {
            int numOfObjects = rnd.Next(1, _spawnPoints.Length);

            for (int i = 0; i < numOfObjects; i++)
            {
                int index = rnd.Next(_spawnPoints.Length);
                Vector3 position = _spawnPoints[index].transform.position;
                Instantiate(prefab, position, Quaternion.identity);
            }
        }
    }
}
