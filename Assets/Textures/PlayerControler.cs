using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private int speed;
    private int healthPoints;
    private int maxHealthPoints;


    public static readonly string LEFT = KeyCode.A.ToString();
    public static readonly string RIGHT = KeyCode.A.ToString();
    public static readonly string UP = KeyCode.A.ToString();
    public static readonly string DOWN = KeyCode.A.ToString();

    public static readonly string ACTION = KeyCode.A.ToString();



    // Use this for initialization
    void Start()
    {
        speed = 5;
        healthPoints = 50;
        maxHealthPoints = 200;
    }

    // Update is called once per frame
    void Update()
    {
        onKeyInput();
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
        Debug.Log("Gracz nacinal przycisk [A]");
    }

    private void onMoveRight()
    {
        // TODO: ruch w prawo implementacja
        Debug.Log("Gracz nacinal przycisk [D]");
    }

    private void onMoveUp()
    {
        // TODO: ruch w gore implementacja
        Debug.Log("Gracz nacinal przycisk [W]");
    }

    private void onMoveDown()
    {
        // TODO: ruch w dol implementacja
        Debug.Log("Gracz nacinal przycisk [S]");
    }

    private void onPlayerAction()
    {
        // TODO: jakies strzelanie, wykonanie akcji
        Debug.Log("Gracz nacinal przycisk [SPACE]");
    }
}
