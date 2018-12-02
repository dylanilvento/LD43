using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;

public class PlayerController : MonoBehaviour {

	int playerId = 0; // The Rewired player id of this character
	private Player player; // The Rewired Player
	Rigidbody2D rb;
	GameObject spriteContainer;
	SpriteRenderer sr;

	public GameObject arrow;

	PlayerTriggerCheck playerTrigger;

	[Range(0, 50)]
	public float maxGravity;

	[Range(0, 10)]
	public float maxGravDistance;

	[Range(0, 10)]
	public float moveSpeed;

	[Range(0, 5)]
	public float arrowRecharge;

	[Range(0, 10)]
	public float arrowVelocity;
	public float moveDirection;

	bool upperHemisphere = true;

	bool canMove = false;

	Vector2 startVector;
	Vector2 currentVector;

	// bool useGravity = true;

	public GameObject worldCenter;

	float directionModifier = 1f;

	// Use this for initialization
	void Start () {
		// Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);

		spriteContainer = transform.GetChild(1).gameObject;
		sr = spriteContainer.GetComponent<SpriteRenderer>();

		rb = GetComponent<Rigidbody2D>();
		startVector = transform.position;

		playerTrigger = transform.GetChild(0).GetComponent<PlayerTriggerCheck>();
	}
	
	// Update is called once per frame
	void Update () {


		// LayerMask colliderMask = LayerMask.GetMask("Platforms");
		// Debug.DrawRay(transform.position, -transform.up, Color.blue, 10f);
		// RaycastHit2D downHit = Physics2D.Raycast(transform.position, -transform.up, 0.5f, colliderMask);
		// print(downHit);

		// print(useGravity);

		moveDirection = player.GetAxis("Move");

		currentVector = transform.position;

		directionModifier = transform.position.x >= worldCenter.transform.position.x ? -1f : 1f;

		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, directionModifier * Vector2.Angle(Vector2.up, currentVector));


		/***********************
			STOPPING MOVEMENT
			AND REVOLUTION
		************************/

		if ((player.GetNegativeButtonUp("Move") || player.GetButtonUp("Move"))) {
			rb.velocity = new Vector2(0f,0f);

			if (transform.position.y > worldCenter.transform.position.y) {
				upperHemisphere = true;
			}

			else {
				upperHemisphere = false;
			}
	
		}

		else if (moveDirection == 0 && playerTrigger.onGround) {
			rb.velocity = new Vector2(0f,0f);
			print("Zero");
		}

		/***********************
			MOVEMENT
		************************/
		if (playerTrigger.onGround) {canMove = true;}
		else {canMove = false;}

		if (player.GetButton("Move") && canMove) {

			// Vector2 right = transform.right;

			if (upperHemisphere) {
				rb.velocity = transform.right * moveSpeed;
				spriteContainer.transform.localScale = new Vector2(Mathf.Abs(spriteContainer.transform.localScale.x), spriteContainer.transform.localScale.y);
			
			}
			else { 
				rb.velocity = transform.right * -moveSpeed;
				spriteContainer.transform.localScale = new Vector2(-Mathf.Abs(spriteContainer.transform.localScale.x), spriteContainer.transform.localScale.y);
			}
			
			// if (!playerTrigger.onGround) UseGravity();

		}

		else if (player.GetNegativeButton("Move") && canMove) {
			
			if (upperHemisphere) {
				rb.velocity = transform.right * -moveSpeed;
				spriteContainer.transform.localScale = new Vector2(-Mathf.Abs(spriteContainer.transform.localScale.x), spriteContainer.transform.localScale.y);
			
			}
			else { 
				rb.velocity = transform.right * moveSpeed;
				spriteContainer.transform.localScale = new Vector2(Mathf.Abs(spriteContainer.transform.localScale.x), spriteContainer.transform.localScale.y);
			}

			// if (!playerTrigger.onGround) UseGravity();
			
		}

		/***********************
			CALLING GRAVITY
		************************/

		if (player.GetButtonDown("Shoot")) {

			StartCoroutine("ShootArrow");

		}
		
	}

	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate()
	{
		if (!playerTrigger.onGround) UseGravity();
	}

	IEnumerator ShootArrow () {
		GameObject spawnedArrow = (GameObject) Instantiate(arrow, transform.position, Quaternion.identity);
	
		if (spriteContainer.transform.localScale.x > 0) {
			spawnedArrow.GetComponent<Rigidbody2D>().velocity = transform.right * arrowVelocity;
		}

		else if (spriteContainer.transform.localScale.x < 0) {
			spawnedArrow.GetComponent<Rigidbody2D>().velocity = -transform.right * arrowVelocity;
		}

		canMove = false;

		yield return new WaitForSeconds(0.2f);

		spawnedArrow.GetComponent<CapsuleCollider2D>().enabled = true;

		yield return new WaitForSeconds(0.1f);

		canMove = true;
	}

	void UseGravity()
	{
		// for(var planet : GameObject in planets) {
        float distance = Vector3.Distance(worldCenter.transform.position, transform.position);
        if (distance <= maxGravDistance) {
            Vector2 v = worldCenter.transform.position - transform.position;
			Vector2 force = v.normalized * (1.0f - distance / maxGravDistance) * maxGravity;

            rb.AddForce(force);
			// print(force);
        }
    //  }
	}
}
