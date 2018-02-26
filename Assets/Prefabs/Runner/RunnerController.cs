using System;
using UnityEngine;

// TODO:
// * optimize layers for colliders and raycast
// * RAYCAST to quad

public class RunnerController : MonoBehaviour
{
	public float ForwardSpeed = 3;
	public float SideSpeed = 1;

	private GameObject leftStoper;
	private GameObject rightStoper;

	private float? sideMoveDirection;
	private const float MaxRaycastLen = 100f;
	private const int TrackLayer = 7;

	void Awake()
	{
		Stoper[] stopers = GetComponentsInChildren<Stoper>();
		if (Vector3.Dot(stopers[0].transform.position - transform.position, transform.right) > 0)
		{
			rightStoper = stopers[0].gameObject;
			leftStoper = stopers[1].gameObject;
		}
		else
		{
			rightStoper = stopers[1].gameObject;
			leftStoper = stopers[0].gameObject;
		}
	}

	void Update ()
	{
		float? horizontal;
		float? vertical;
		InputController.GetmovementInfo(out horizontal, out vertical);
		if (horizontal.HasValue)
		{
			Vector3 raycastDirection = horizontal.Value * Vector3.right;

			RaycastHit raycastHit;
			if (Physics.Raycast(
				transform.position + raycastDirection, raycastDirection, out raycastHit))
			{
				Debug.Log(raycastHit.collider.gameObject.name);
				sideMoveDirection = horizontal;

				if (horizontal.Value > 0)
				{
					rightStoper.SetActive(false);
					leftStoper.SetActive(true);
				}
				else
				{
					leftStoper.SetActive(false);
					rightStoper.SetActive(true);
				}
			}
		}

		MakeMove();
	}

	private void MakeMove()
	{
		Vector3 displacement = Time.deltaTime * ForwardSpeed * transform.forward;
		if (sideMoveDirection.HasValue)
		{
			displacement += Time.deltaTime * SideSpeed * sideMoveDirection.Value * Vector3.right;
		}
		transform.position += displacement;
	}

	internal void StopSideMotion(Vector3 newForward)
	{
		sideMoveDirection = null;

		leftStoper.SetActive(true);
		rightStoper.SetActive(true);

		transform.forward = newForward;
	}

	class InputController
	{
		public static void GetmovementInfo(out float? horizontal, out float? vertical)
		{
			horizontal = null;
			if (Input.GetKey(KeyCode.A))
			{
				horizontal = -1f;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				horizontal = +1f;
			}

			vertical = null;
			if (Input.GetKey(KeyCode.S))
			{
				vertical = -1f;
			}
			else if (Input.GetKey(KeyCode.W))
			{
				vertical = +1f;
			}
		}
	}
}
