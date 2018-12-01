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
	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D>();
		startVector = transform.position;

		worldCenter = GameObject.Find("Center");
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 currentVector = transform.position;

		float radius = Vector2.Distance(transform.position, worldCenter.transform.position);
		float theta = Vector2.Angle(startVector, currentVector);

		float xPos = radius * Mathf.Sin(theta) * Time.deltaTime;
		float yPos = radius * Mathf.Cos(theta) * Time.deltaTime;


		float directionModifier = transform.position.x >= worldCenter.transform.position.x ? -1f : 1f;

		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, directionModifier * Vector2.Angle(startVector, currentVector));

		// UseGravity();
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
