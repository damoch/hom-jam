using UnityEngine;

public class CameraControler : MonoBehaviour {

    public GameObject Player;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        hookCameraToPlayer();
    }

    private void hookCameraToPlayer()
    {
        float posX = Player.transform.position.x;
        float posY = Player.transform.position.y;
        transform.position = new Vector3(posX, posY, transform.position.z);
    }
}
