﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class minigame3Start : NetworkBehaviour {

	private bool eventEnabled;
	private bool packageNearby;
	private int playerCount;

	public GameObject minigame3Player;

	void Start(){
		Eventmanager.Instance.EventonMinigame3Activated += HandleEventonMinigame3Activated;
		Gamemanager.Instance.onDisableEventHandlers += OnDisable;
		eventEnabled = true;
	}

	void OnDisable(){
		if (eventEnabled) {
			Eventmanager.Instance.EventonMinigame3Activated -= HandleEventonMinigame3Activated;
			eventEnabled = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "PickUp1" || other.tag == "PickUpMagic") {
			packageNearby = true;
		}
		if (other.tag == "Player") {
			playerCount += 1;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "PickUp1" || other.tag == "PickUpMagic") {
			packageNearby = false;
		}
		if (other.tag == "Player") {
			playerCount -= 1;
		}
	}

	[Server]
	void HandleEventonMinigame3Activated(){
		Debug.Log ("Starting minigame3");
		if (playerCount == 2) {
			GameNetworkManager.singleton.playerPrefab = minigame3Player;
			Eventmanager.Instance.triggerLevelSwitch ("Minigame3");
		}
	}
}
