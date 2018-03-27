using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneScript : MonoBehaviour {

	public float HangingHeight = 2;
	public float VerticalSpeed = 2;

	public GameObject Target;
		
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < HangingHeight)
		{
			transform.position += Time.deltaTime * new Vector3(0, VerticalSpeed, 0);
		}

		transform.up = Vector3.RotateTowards(
			transform.up,
			Target.transform.position - transform.position,
			0.02f, 0f);
	}
}
