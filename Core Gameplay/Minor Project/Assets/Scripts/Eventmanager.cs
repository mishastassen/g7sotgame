﻿using UnityEngine;
using System.Collections;

public class Eventmanager : MonoBehaviour {
	
	private static Eventmanager static_instance = null;

	//Events:
	//Playeradded event
	public delegate void PlayerAdded(GameObject player);
	public static event PlayerAdded onPlayerAdded;

	//PlayerRemoved event
	public delegate void PlayerRemoved(GameObject player);
	public static event PlayerRemoved onPlayerRemoved;

	//Function to call this object
	public static Eventmanager Instance{
		get{
			if(static_instance == null){
				static_instance = GameObject.FindObjectOfType(typeof(Eventmanager)) as Eventmanager;
			}
			return static_instance;
		}
	}

	//Trigger PlayerAdded event
	public void triggerPlayerAdded(GameObject player){
		if (onPlayerAdded != null) {	//Don't execute if noone is listening to event
			onPlayerAdded(player);
		}
	}

	//Trigger PlayerRemoved event
	public void triggerPlayerRemoved(GameObject player){
		if (onPlayerRemoved != null) {	//Don't execute if noone is listening to event
			onPlayerRemoved(player);
		}
	}


}