using System;
using UnityEngine;

internal class SideMotionController
{
    private readonly float sideSpeed;
	private float currentSpeed;
	private GameObject leftStoper;
    private GameObject rightStoper;

    public SideMotionController(float sideSpeed, Transform transform)
    {
        this.sideSpeed = sideSpeed;
        Stoper[] stopers = transform.GetComponentsInChildren<Stoper>();
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
		leftStoper.SetActive(true);
		rightStoper.SetActive(true);
	}

    internal float GetTorightSpeed()
    {
		return currentSpeed;
    }

	internal void StartSideMotion(float moveDirection)
	{
		currentSpeed = Math.Sign(moveDirection) * sideSpeed;
		if (currentSpeed > 0)
		{
			rightStoper.SetActive(false);
			leftStoper.SetActive(true);
		}
		if (currentSpeed < 0)
		{
			leftStoper.SetActive(false);
			rightStoper.SetActive(true);
		}
	}

	internal void StopSideMotion()
	{
		currentSpeed = 0;
		leftStoper.SetActive(true);
		rightStoper.SetActive(true);
	}
}