﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerB : MonoBehaviour {
	
	public Text timerText;
	private float timer;
	private float minutes;
	private string seconds;
	private bool levelFinished;
	
	private bool enabled;
	
	void Start () {
		timer = Gamemanager.Instance.timer;
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
		minutes = Mathf.Floor (timer / 60);
		seconds = (timer % 60).ToString ("00");
		timerText.text = minutes + ":" + seconds;
	}
}