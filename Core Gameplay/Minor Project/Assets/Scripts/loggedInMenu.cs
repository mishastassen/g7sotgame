﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class loggedInMenu : MonoBehaviour {

	public Canvas loggedIn;
	public Canvas network;
	public Canvas mainMenu;

	public Text usernameText;

	public void pressLogOut(){
		WebManager.Instance.logout ();
	}

	void Update(){
		if (loggedIn.enabled == true && WebManager.Instance.currentUser == null && mainMenu.enabled == false) {
			network.enabled = true;
			loggedIn.enabled = false;
		}
	}
}