using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalPull : MonoBehaviour {
	[Range(0, 100)]
	public float maxGravity;

	// [Range(0, 40)]
	float maxGravDistance = 1000f;
	GameObject worldCenter;
	
	Vector2 startVector;

	Rigidbody2D rb;

	bool useGravity = true;
	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D>();
		startVector = transform.position;

		worldCenter = GameObject.Find("Center");
		
	}
	
	// Update is called once per frame
	void Update () {
		UseGravity();
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
