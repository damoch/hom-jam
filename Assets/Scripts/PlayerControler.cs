using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [Range(0, 100)]
    public float speed;
    [Range(0, 100)]
    public int healthPoints;
    [Range(0, 100)]
    public int maxHealthPoints;
    [Range(0, 100)]
    public int highHealth;
    [Range(0, 100)]
    public int lowHealth;
    [Range(0, 100)]
    public int minHealthPoints;
    [Range(0, 10)]
    public float shootCooldown;
    public bool canShoot;
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPoint;

    private float moveX;
    private float moveY;
    public Rigidbody2D rigidboy;
    private Vector2 mousePosition;


    public static readonly KeyCode LEFT = KeyCode.A;
    public static readonly KeyCode RIGHT = KeyCode.D;
    public static readonly KeyCode UP = KeyCode.W;
    public static readonly KeyCode DOWN = KeyCode.S;

    public static readonly string ACTION = "Fire1";



    // Use this for initialization
    void Start()
    {
        rigidboy = GetComponent<Rigidbody2D>();
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        onKeyInput();
        playerMoving();
        changePlayerLookingDirection();
    }

    private void playerMoving()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        rigidboy.velocity = new Vector2(moveX * speed, moveY * speed);
    }

    private void onKeyInput()
    {
        if (Input.GetKeyDown(LEFT))
            onMoveLeft();

        if (Input.GetKeyDown(RIGHT))
            onMoveRight();

        if (Input.GetKeyDown(UP))
            onMoveUp();

        if (Input.GetKeyDown(DOWN))
            onMoveDown();

        if (Input.GetMouseButtonDown(0))
            onPlayerAction();
    }


    private void onMoveLeft()
    {
        // TODO: ruch w lewo implementacja 
    }

    private void onMoveRight()
    {
        // TODO: ruch w prawo implementacja
    }

    private void onMoveUp()
    {
        // TODO: ruch w gore implementacja
    }

    private void onMoveDown()
    {
        // TODO: ruch w dol implementacja

    }

    private void onPlayerAction()
    {
        // TODO: jakies strzelanie, wykonanie akcji
        playerShoot();
    }

    private void changePlayerLookingDirection()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        transform.up = direction;
    }

    private void playerShoot()
    {
        if (canShoot)
        {
            Debug.Log("Player shoot");
            createAndShootBullet();
            StartCoroutine("waitShootCoolDownTime");
        }
    }

    private IEnumerator waitShootCoolDownTime()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    private void createAndShootBullet()
    {
        Instantiate(bulletPrefab, bulletSpawnPoint.transform.position ,transform.rotation);
    }

}
