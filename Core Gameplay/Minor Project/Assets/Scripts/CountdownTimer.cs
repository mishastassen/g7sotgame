﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimer : NetworkBehaviour {

	public Text timerText;
	public float startTime;
	private float timer;
	private float time;
	private float minutes;
	private string seconds;
	private bool levelFinished;

	private bool enabled;
	
	void Start () {
		Eventmanager.Instance.EventonLevelFinished += HandleEventonLevelFinished;
		timerText.text = "";
		minutes = 0.0f;
		seconds = "";
		levelFinished = false;
	}

	void OnEnable() {
		Eventmanager.Instance.EventonLevelFinished += HandleEventonLevelFinished;
		enabled = true;
		Gamemanager.Instance.onDisableEventHandlers += OnDisable;
	}
	
	void OnDisable() {
		if (enabled) {
			Eventmanager.Instance.EventonLevelFinished -= HandleEventonLevelFinished;
			enabled = false;
		}
	}
	
	void Update () {
		if (!levelFinished) {
			UpdateTimer ();
		}
	}
	
	void HandleEventonLevelFinished (string nextLevel)
	{	
		levelFinished = true;
	}
	
	void UpdateTimer() {
		timer += Time.deltaTime;
		time = startTime - timer;
		minutes = Mathf.Floor (time / 60);
		seconds = (time % 60).ToString ("00");
		timerText.text = minutes + ":" + seconds;
	}
}
