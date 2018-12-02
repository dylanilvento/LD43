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

	Animator animator;

	public GameObject rightArrow, leftArrow, sweat;

	PlayerTriggerCheck playerTrigger;

	[Range(0, 50)]
	public float maxGravity;

	[Range(0, 10)]
	public float maxGravDistance;

	[Range(0, 10)]
	public float moveSpeed;

	[Range(0, 5)]
	public float arrowRecharge;

	float arrowTimer = 0f;

	[Range(0, 50)]
	public float arrowVelocity;
	public float moveDirection;

	bool upperHemisphere = true;

	bool canMove = false;

	Vector2 startVector;
	Vector2 currentVector;

	Vector2 originalPosBeforeShake;

	// bool useGravity = true;

	public GameObject worldCenter;

	float directionModifier = 1f;

	// Use this for initialization
	void Start () {
		// Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);

		spriteContainer = transform.GetChild(1).gameObject;
		sr = spriteContainer.GetComponent<SpriteRenderer>();
		animator = spriteContainer.GetComponent<Animator>();

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
			animator.SetBool("Move", false);

			if (transform.position.y > worldCenter.transform.position.y) {
				upperHemisphere = true;
			}

			else {
				upperHemisphere = false;
			}
	
		}

		else if (moveDirection == 0 && playerTrigger.onGround) {
			rb.velocity = new Vector2(0f,0f);
			// print("Zero");
		}

		/***********************
			MOVEMENT
		************************/
		if (playerTrigger.onGround && !player.GetButton("Shoot")) {canMove = true;}
		else {canMove = false;}

		if (player.GetButton("Move") && canMove && !player.GetButton("Shoot"))  {

			animator.SetBool("Move", true);

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

		else if (player.GetNegativeButton("Move") && canMove && !player.GetButton("Shoot")) {

			animator.SetBool("Move", true);
			
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
			SHOOTING ARROWS
			FREEZE MOVEMENT ON GROUND CHECK ^
		************************/

		if (player.GetButton("Shoot")) {

			// canMove = false;

			// print(arrowTimer);

			// StartCoroutine("ShootArrow");
			arrowTimer += Time.deltaTime;

			if (arrowTimer < 0.5f) {
				animator.SetBool("Shoot1", true);
				// animator.SetBool("Move", false);
			}

			else if (arrowTimer >= 0.5f && arrowTimer < 1.5f) {
				animator.SetBool("Shoot2", true);
				animator.SetBool("Shoot1", false);
			}

			else if (arrowTimer >= 1.5f && arrowTimer < 2.5f) {
				animator.SetBool("Shoot3", true);
				animator.SetBool("Shoot2", false);
			}

			else if (arrowTimer >= 2.5f && arrowTimer <= 3.5f) {
				animator.SetBool("Shoot4", true);
				animator.SetBool("Shoot3", false);
			}

			else if (arrowTimer >= 3.5f) {
				originalPosBeforeShake = transform.position;
				Shake();
			}

			//do Shoots 1 through 4.

		}

		if (player.GetButtonUp("Shoot")) {
			arrowTimer = 0f;

			animator.SetBool("Shoot1", false);
			animator.SetBool("Shoot2", false);
			animator.SetBool("Shoot3", false);
			animator.SetBool("Shoot4", false);


			if (spriteContainer.transform.localScale.x > 0) {
			
				StartCoroutine(ShootArrow(rightArrow, 1f));
			}

			else if (spriteContainer.transform.localScale.x < 0) {
				
				StartCoroutine(ShootArrow(leftArrow, -1f));

			}
		}
		
	}

	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate()
	{
		if (!playerTrigger.onGround) UseGravity();
	}

	void Shake () {
		
		float magnitude = 0.01f;
    
		float damper = 1.0f - UnityEngine.Random.value;
		
		float x = UnityEngine.Random.value * 2.0f - 1.0f;
		float y = UnityEngine.Random.value * 2.0f - 1.0f;
		x *= magnitude * damper;
		y *= magnitude * damper;
		
		transform.position = new Vector2(originalPosBeforeShake.x + x, originalPosBeforeShake.y + y);

		if (UnityEngine.Random.Range(0f, 1f) > 0.65f) {
			GameObject spawnedSweat = (GameObject) Instantiate(sweat, transform.position + (transform.up/3),Quaternion.identity);
			spawnedSweat.GetComponent<Rigidbody2D>().velocity = new Vector2(UnityEngine.Random.Range(-transform.right.x * 4f, transform.right.x * 4f), UnityEngine.Random.Range(transform.up.y/2f, transform.up.y));
			UnityEngine.Object.Destroy(spawnedSweat, UnityEngine.Random.Range(0.1f, 0.5f));
		}
		
	            
	}

	IEnumerator ShootArrow (GameObject arrowInstantiatedObject, float velocityModifier) {


		GameObject spawnedArrow = (GameObject) Instantiate(arrowInstantiatedObject, transform.position, Quaternion.identity);
			
		spawnedArrow.GetComponent<Rigidbody2D>().velocity = transform.right * arrowVelocity * velocityModifier;
		
		// spawnedArrow.transform.GetChild(0).localScale = new Vector2(-1, 1);

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
