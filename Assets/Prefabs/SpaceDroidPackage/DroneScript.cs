using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneScript : MonoBehaviour {

	public float HangingHeight = 2;
	public float VerticalSpeed = 2;

	public GameObject Target;
	public DronFactory Owner;

	// Use this for initialization
	void Start () {
		transform.rotation = Quaternion.Euler(-90, 0, 0);
	}

	float GetZRotOnTarget()
	{
		Vector3 targetDirection = Target.transform.position - transform.position;
		targetDirection.y = 0;

		float a1 = Vector3.Angle(Vector3.forward, targetDirection);

		float a2 = Vector3.Angle(Vector3.right, targetDirection);
		a2 = a2 - 90 <= 0 ? +1 : -1;
		return a2 * a1;
	}

	internal void Die()
	{
		Owner.FreeDrone(this);
	}

	// Update is called once per frame
	void Update () {

		Vector3 position = transform.position;
		float delta = HangingHeight - transform.position.y;
		if (Mathf.Abs(delta) > 0.01f)
		{
			position.y = position.y + Mathf.Sign(delta) * VerticalSpeed * Time.deltaTime;
			transform.position = position;
		}
		transform.rotation = Quaternion.Euler(-90, 0, GetZRotOnTarget());
	}
}
