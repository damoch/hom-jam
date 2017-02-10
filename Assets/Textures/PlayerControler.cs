using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private float speed;
    private int healthPoints;
    private int maxHealthPoints;
    private float moveX;
    private float moveY;
    public Rigidbody2D rigidboy;
    private Vector2 mousePosition;


    public static readonly KeyCode LEFT = KeyCode.A;
    public static readonly KeyCode RIGHT = KeyCode.A;
    public static readonly KeyCode UP = KeyCode.A;
    public static readonly KeyCode DOWN = KeyCode.A;

    public static readonly KeyCode ACTION = KeyCode.A;



    // Use this for initialization
    void Start()
    {
        speed = 10f;
        healthPoints = 50;
        maxHealthPoints = 200;
        rigidboy = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        onKeyInput();
        changePlayerLookingDirection();

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

        if (Input.GetKeyDown(ACTION))
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
    }

    private void changePlayerLookingDirection()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        transform.up = direction;
    }

}
