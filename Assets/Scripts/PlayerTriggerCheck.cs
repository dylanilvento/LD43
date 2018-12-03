using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerCheck : MonoBehaviour {
	public bool onGround = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Sent when another object enters a trigger collider attached to this
	/// object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		onGround = true;
	}

	/// <summary>
	/// Sent when another object leaves a trigger collider attached to
	/// this object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerExit2D(Collider2D other)
	{
		onGround = false;
	}

	void OnTriggerStay2D(Collider2D other)
	{
		onGround = true;
	}

	
}
