﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PickUp1Controller : NetworkBehaviour {

	public GameObject PickUp1SpawnPrefab;

	private bool isDestroyed = false;
	// Use this for initialization
	void Start () {
		if (isServer) {
			if(GameObject.FindWithTag("PickUp1Spawn") == null){
				GameObject newSpawn = (GameObject)Instantiate (PickUp1SpawnPrefab, this.transform.position, this.transform.rotation);
				NetworkServer.Spawn (newSpawn);
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (isServer && other.tag == "DeathZone" && !isDestroyed) {
			isDestroyed = true;
			Eventmanager.Instance.triggerPackageDestroyed();
		}
	}

}