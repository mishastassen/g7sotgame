﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PatrollingGuard: NetworkBehaviour {

	private Rigidbody enemy;
	private float speed;
	private bool facingRight;

	private Vector3 eyePosition;
	private bool spotted;
	private bool enabled;
	private float coneDegrees; //halve hoek van gezichtspunt gegeven in graden
	private Vector3 direction;
	private float angle;
	RaycastHit hitInfo;
	private Animator anim;

	private GameObject targetPlayer;
	private Vector3 playerPos;
	private float strikingDistance;
	private bool waiting;

	// attributen voor het zorgen dat hij niet meer moonwalkt
	public GameObject front;
	public GameObject back;

	void Start () {
		enemy = GetComponent<Rigidbody>();
		speed = 4;
		facingRight = false;
		coneDegrees = 60;
		spotted = false;
		anim = GetComponent<Animator> ();
		strikingDistance = 12f;
		waiting = false;
	}
		
	void Update (){
		if (spotted) {
			CmdPlayerSpotted ();
			bool shouldStrike = IsInStrikingDistance (playerPos);
			Strike (shouldStrike);
		} else {
			anim.speed = 1f;
			CmdNoPlayerSpotted ();
			walk ();
		}

		if (this.GetComponent<Rigidbody> ().velocity.x > 0) {
			if (front.transform.position.x < back.transform.position.x) {
				flip ();
			}
		} else if (this.GetComponent<Rigidbody> ().velocity.x < 0) {
			if (front.transform.position.x > back.transform.position.x) {
				flip ();
			}
		}
	}

	bool IsInStrikingDistance(Vector3 playerPos) {
		Vector3 curPos = this.transform.position;
		targetPlayer = GameObject.FindGameObjectWithTag ("Player");
		playerPos = targetPlayer.transform.position;
		return  Vector3.Distance (curPos, playerPos) < strikingDistance;
	}

	void Strike(bool shouldStrike) {
		if (shouldStrike) {
			if (!waiting) {
				StartCoroutine (WaitBeforeHit ());
			}
		}
		else{
			if (anim.GetBool ("isStriking")) {
				RpcStopStrike ();
			} else {
				walk ();
			}
		}
	}

	void walk (){
		bool groundedRight = isGroundedRight ();
		bool groundedLeft = isGroundedLeft ();

		if (groundedLeft && groundedRight) {
			if (facingRight) {
				walkRight ();
				eyePosition = new Vector3 (enemy.transform.position.x + 0.5f, enemy.transform.position.y + 2.0f, enemy.transform.position.z);
			} else {
				walkLeft ();
				eyePosition = new Vector3 (enemy.transform.position.x - 0.5f, enemy.transform.position.y + 2.0f, enemy.transform.position.z);
			}
		} else if (groundedLeft && !groundedRight) {
			flip ();
			walkLeft ();
			facingRight = false;
		} else if (!groundedLeft && groundedRight) {
			flip ();
			walkRight ();
			facingRight = true;
		} else {
		}
	}

	// checks whether the rigth of the enemy is on a platform
	bool isGroundedRight() {
		Vector3 rightPosition = new Vector3 (enemy.transform.position.x + 0.9f, enemy.transform.position.y + 1, enemy.transform.position.z);
		return Physics.Raycast (rightPosition, Vector3.down, 6);
	}

	// checks whether the left of the enemy is on a platform
	bool isGroundedLeft() {
		Vector3 leftPosition= new Vector3 (enemy.transform.position.x - 0.9f, enemy.transform.position.y + 1, enemy.transform.position.z);
		return Physics.Raycast (leftPosition, Vector3.down, 6);
	}	

	void walkRight() {
		float yVelocity = enemy.velocity.y;
		Vector3 movement = new Vector3 (speed, yVelocity, 0.0f);
		enemy.velocity = movement;
	}

	void walkLeft() {
		float yVelocity = enemy.velocity.y;
		Vector3 movement = new Vector3 (speed * -1, yVelocity, 0.0f);
		enemy.velocity = movement;
	}

	void flip () {
		Vector3 theScale = enemy.transform.localScale;
		theScale.z *= -1;
		transform.localScale = theScale;
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Player") {
			
			direction = other.transform.position + new Vector3(0,3,0) - eyePosition;

			if (facingRight) {
				angle = Vector3.Angle (direction, Vector3.right);
			} else {
				angle = Vector3.Angle (direction, Vector3.left);
			}

			if (angle <= coneDegrees) {

				if (Physics.Raycast (eyePosition, direction, out hitInfo)) {
					if (hitInfo.collider.tag == "Player") {
						Debug.DrawRay (eyePosition, direction, Color.red);
						spotted = true;
						playerPos = other.transform.position;
					} else {
						Debug.DrawRay(eyePosition, direction, Color.green);
						spotted = false;
					}
				} else {				
					Debug.DrawRay(eyePosition, direction, Color.green);
					spotted = false;
				}
			} else {
				Debug.DrawRay(eyePosition, direction, Color.green);
				spotted = false;
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			spotted = false;
		}
	}

	[Command]
	void CmdPlayerSpotted() {
		Eventmanager.Instance.triggerPlayerSpotted();
	}

	[Command]
	void CmdNoPlayerSpotted() {
		Eventmanager.Instance.triggerNoPlayerSpotted ();
	}

	IEnumerator WaitBeforeHit(){
		waiting = true;
		Debug.Log ("Wait for striking");
		yield return new WaitForSeconds(1f);
		anim.speed = 1f;
		RpcStartStrike ();
		waiting = false;
	}

	[ClientRpc]
	void RpcStartStrike(){
		anim.SetBool ("isStriking", true);
	}

	[ClientRpc]
	void RpcStopStrike(){
		anim.SetBool ("isStriking", false);
	}
}
