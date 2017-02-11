using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    private EnemyController enemyController;
    void Start()
    {
        enemyController = transform.parent.gameObject.GetComponent<EnemyController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    { 
        enemyController.TriggerEnter(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        enemyController.TriggerExit(other);
    }
}
