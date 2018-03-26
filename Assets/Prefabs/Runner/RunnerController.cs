using System;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
	public float NormalForwardSpeed = 5;
    public float NormalForwardSpeedRestoringAxeleration = 1;
    public float AfterCollisionForwardSpeed = 2;
    public float SideSpeed = 2;

    private ForwardSpeedController forwardSpeedController;
    private SideMotionController sideMotionController;
	private ForwardDirectionController forwardDirectionController;

	void Awake()
    {
        forwardSpeedController = new ForwardSpeedController(NormalForwardSpeed, NormalForwardSpeedRestoringAxeleration);
        sideMotionController = new SideMotionController(SideSpeed, transform);
		forwardDirectionController = new ForwardDirectionController(transform);
	}

	void Update ()
	{
		float? horizontal;
		float? vertical;
		InputController.GetmovementInfo(out horizontal, out vertical);
		if (horizontal.HasValue)
		{
			if (onCollisionCount == 0 && TrackFinder.ExistTrackInDirection(transform, horizontal.Value))
			{
				sideMotionController.StartSideMotion(horizontal.Value);
			}
		}
		MakeMove();
	}

	private void MakeMove()
	{
		transform.position += Time.deltaTime *
            (forwardSpeedController.GetForwardSpeedAndTick() * forwardDirectionController.GetMoveDirectionAntTick() +
            sideMotionController.GetTorightSpeed() * transform.right);
	}

	internal void StopSideMotion(Vector3 newForward)
	{
		sideMotionController.StopSideMotion();
		forwardDirectionController.SetDirection(newForward);
	}

	int onCollisionCount;

	internal void CollideWithBarier()
	{
		onCollisionCount++;
		forwardSpeedController.SetMaxCurrentSppen(AfterCollisionForwardSpeed);
		sideMotionController.StartSideMotion(-sideMotionController.GetTorightSpeed());
	}

	internal void StopCollision()
	{
		onCollisionCount--;
	}
}