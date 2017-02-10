﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour {

    GameObject player;


	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        hookCameraToPlayer();
    }

    private void hookCameraToPlayer()
    {
        float posX = player.transform.position.x;
        float posY = player.transform.position.y;
        Debug.Log("Player x=" + posX + " y=" + posY);
        transform.position = new Vector3(posX, posY, transform.position.z);
    }
}