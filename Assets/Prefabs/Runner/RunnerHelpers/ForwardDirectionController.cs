using System;
using UnityEngine;

class ForwardDirectionController
{
	private readonly Transform transform;
	private Vector3 currentDirection;

	public ForwardDirectionController(Transform transform)
	{
		currentDirection = transform.forward;
		this.transform = transform;
	}

	internal Vector3 GetMoveDirectionAntTick()
	{
		//float currentAngle = Vector3.Angle(transform.forward, currentDirection);
		transform.forward = Vector3.RotateTowards(transform.forward, currentDirection, 0.01f, 0f);
		return currentDirection;
	}

	internal void SetDirection(Vector3 newForward)
	{
		currentDirection = newForward;
	}
}