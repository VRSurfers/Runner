using System;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
	public float NormalForwardSpeed = 5;
	public float NormalForwardSpeedRestoringAxeleration = 1;
	public float AfterCollisionForwardSpeed = 2;
	public float SideSpeed = 2;
	public AudioSource CollisionSound;

	private ForwardSpeedController forwardSpeedController;
	private SideMotionController sideMotionController;
	private ForwardDirectionController forwardDirectionController;
	private float hp;

	public float HP
	{
		get { return hp; }
		set { hp = value; }
	}

	void Awake()
	{
		forwardSpeedController = new ForwardSpeedController(NormalForwardSpeed, NormalForwardSpeedRestoringAxeleration);
		sideMotionController = new SideMotionController(SideSpeed, transform);
		forwardDirectionController = new ForwardDirectionController(transform);
	}

	void Update()
	{
		float? horizontal;
		float? vertical;
		InputController.GetmovementInfo(out horizontal, out vertical);
		if (horizontal.HasValue && onCollisionCount == 0)
		{
			Vector3? trackDirection = TrackFinder.ExistTrackInDirection(transform, horizontal.Value);
			if (trackDirection.HasValue)
			{
				forwardDirectionController.SetDirection(trackDirection.Value);
				sideMotionController.StartSideMotion(horizontal.Value);
			}
		}
		MakeMove();
		Debug.Log(hp);
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
		CollisionSound.Play();
	}

	internal void StopCollision()
	{
		onCollisionCount--;
	}
}