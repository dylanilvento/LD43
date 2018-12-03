using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
	[Range(0, 100)]
	public float maxGravity;

	[Range(0, 40)]
	public float maxGravDistance;

	GameObject worldCenter;
	
	Vector2 startVector;

	Rigidbody2D rb;

	bool useGravity = false;
	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D>();
		startVector = transform.position;

		worldCenter = GameObject.Find("Center");
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 currentVector = transform.position;

		float directionModifier = transform.position.x >= worldCenter.transform.position.x ? -1f : 1f;

		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, directionModifier * Vector2.Angle(Vector2.up, currentVector));

		// transform.position = new Vector2(xPos, yPos);

	}

	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate()
	{
		if (useGravity) UseGravity();
	}

	public void SetUseGravity (bool val) {
		useGravity = val;
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

	/// <summary>
	/// Sent when an incoming collider makes contact with this object's
	/// collider (2D physics only).
	/// </summary>
	/// <param name="other">The Collision2D data associated with this collision.</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		rb.constraints = RigidbodyConstraints2D.FreezeAll;

		useGravity = false;

		Destroy(gameObject);

	}

}
