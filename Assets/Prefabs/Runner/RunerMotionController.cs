using System;
using UnityEngine;

public partial class RunerMotionController : MonoBehaviour
{
	public Transform RunnerModel;
	public MapController MapController;
	public float MaxSideSpeed = 2;
	public float SideCollisionHealthDamage = 30;
	public float ForwardCollisionHealthDamage = 100;


	public ScoresCounter Health;
	public ScoresCounter Amo;
	public ScoresCounter Jumps;

	public AudioSource FastCollision;
	public AudioSource StrongCollision;
	public AudioSource Jump;
	public AudioSource Landing;

	private KickTracker kickTracker;
    private float? sideSpeed;
	private bool isSideOnCollision;

	private void Awake()
	{
		kickTracker = new KickTracker(RunnerModel, Landing);
	}

	public void ProcessMotionStep(ref InputArgs inputArgs)
	{
		if (kickTracker.FlyType != FlyType.None)
		{
			kickTracker.Step();
			return;
		} else if (inputArgs.Jumped && Jumps.Score > 0)
		{
			Jump.Play();
			Jumps.Change(-1);
			kickTracker.Jump();
		}

		float? executedMotion = inputArgs.Horizontal;
		if (isSideOnCollision)
			executedMotion = null; // prevents moving into coolider after rebound

		Move(executedMotion);
	}

	private void Move(float? newSideSpeedSign)
	{
		float leftRowX = MapController.LeftRowX;
		float rowWidth = MapController.RowWidth;
		float rightRowX = leftRowX + rowWidth * (MapController.RowCount - 1);

		Vector3 oldPositon = transform.position;
		float xCurrent = oldPositon.x;
		
		if (newSideSpeedSign.HasValue)
			sideSpeed = newSideSpeedSign * MaxSideSpeed;
		
		if (sideSpeed.HasValue)
		{
			float targetX;
			float maxPotentialDisplacement = Time.deltaTime * sideSpeed.Value;
			if (maxPotentialDisplacement > 0)
			{
				if (xCurrent + maxPotentialDisplacement > rightRowX)
				{
					targetX = rightRowX;
					sideSpeed = null;
				}
				else
				{
					if (newSideSpeedSign == null)
					{
						double xRelative = (xCurrent - leftRowX) / rowWidth;
						double nextRowIndex = Math.Ceiling(xRelative);
						float nextTrackX = (float)(leftRowX + nextRowIndex * rowWidth);
						if (xCurrent + maxPotentialDisplacement > nextTrackX)
						{
							sideSpeed = null;
							targetX = nextTrackX;
						}
						else
							targetX = xCurrent + maxPotentialDisplacement;
					}
					else
						targetX = xCurrent + maxPotentialDisplacement;
				}
			}
			else
			{
				if (xCurrent + maxPotentialDisplacement < leftRowX)
				{
					targetX = leftRowX;
					sideSpeed = null;
				}
				else
				{
					if (newSideSpeedSign == null)
					{
						double xRelative = (xCurrent - leftRowX) / rowWidth;
						double nextRowIndex = Math.Floor(xRelative);
						float nextTrackX = (float)(leftRowX + nextRowIndex * rowWidth);
						if (xCurrent + maxPotentialDisplacement < nextTrackX)
						{
							sideSpeed = null;
							targetX = nextTrackX;
						}
						else
							targetX = xCurrent + maxPotentialDisplacement;
					}
					else
						targetX = xCurrent + maxPotentialDisplacement;
				}
			}
			oldPositon.x = targetX;
			transform.position = oldPositon;
		}
	}

	internal void FrontCollision(float x)
    {
		if (kickTracker.FlyType == FlyType.AfterCash)
			return;

		Health.Change(-ForwardCollisionHealthDamage);
		StrongCollision.Play();
		kickTracker.KickByCar(x);
        sideSpeed = null;
    }

	internal void StopCollision()
	{
		isSideOnCollision = false;
	}

	internal void SideCollision()
    {
		if (isSideOnCollision)
			return;

		isSideOnCollision = true;
		if (kickTracker.FlyType == FlyType.None)
			sideSpeed = -(sideSpeed.Value);
		Health.Change(-SideCollisionHealthDamage);
		FastCollision.Play();
	}
}