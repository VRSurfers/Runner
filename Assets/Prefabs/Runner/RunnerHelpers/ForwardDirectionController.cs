using System;
using UnityEngine;

class ForwardDirectionController
{
	private readonly Transform transform;
	private Vector3 targetDirection;

	public ForwardDirectionController(Transform transform)
	{
		targetDirection = transform.forward;
		this.transform = transform;
	}

	internal Vector3 GetMoveDirectionAntTick()
	{
		transform.forward = Vector3.RotateTowards(transform.forward, targetDirection, 0.01f, 0f);
		return targetDirection;
	}

	internal void SetDirection(Vector3 newForward)
	{
		targetDirection = newForward;
	}
}