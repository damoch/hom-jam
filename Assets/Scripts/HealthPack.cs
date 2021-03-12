using UnityEngine;

public class HealthPack : MonoBehaviour {

    public int HitPoints;
    public float Speed;

    private GameManager gameManager;
    private Rigidbody2D _rb;
    private Vector3 _playerPosition;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 dir = Vector2.right;
        dir *= Speed * Time.fixedDeltaTime;
        gameObject.transform.Translate(dir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Character>().UpdateHealthValue(HitPoints);
            gameManager.HealthKitDestroyed();
            Destroy(gameObject);
        }
        else if (collision.tag == "Boundary")
        {
            gameManager.HealthKitDestroyed();
            Destroy(gameObject);
        }
    }

    public void LookAtPlayer(GameObject player)
    {
        float angle = Mathf.Atan2((player.transform.position.y - transform.position.y), (player.transform.position.x - transform.position.x)) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    internal void MoveToPlayer(PlayerControler player)
    {
        LookAtPlayer(player.gameObject);
        _playerPosition = player.gameObject.transform.position;
    }
}
