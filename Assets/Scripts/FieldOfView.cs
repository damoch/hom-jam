using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    public EnemyController enemyController;
    void Start()
    {
        if (!enemyController)
        {
            enemyController = transform.parent.gameObject.GetComponent<EnemyController>();
        }
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
