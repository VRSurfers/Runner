using System;
using UnityEngine;

public partial class RunerMotionController : MonoBehaviour
{
	public Transform RunnerModel;
	public MapController MapController;
	public float MaxSideSpeed = 2;

	public HealthComponent HealthComponent { get; private set; }
	public AmoComponent AmoComponent { get; private set; }

	private KickTracker kickTracker;
    private float? sideSpeed;
	private bool isOnCollision;

	private void Awake()
	{
		kickTracker = new KickTracker(RunnerModel);
		HealthComponent = GetComponent<HealthComponent>();
		AmoComponent = GetComponent<AmoComponent>();
	}

	public void ProcessMotionStep(ref InputArgs inputArgs)
	{
		float? executedMotion = inputArgs.Horizontal;
		if (kickTracker.IsOnKick)
		{
			kickTracker.Step();
			return;
		}

		if (isOnCollision)
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
		kickTracker.KickTo(x);
        sideSpeed = null;
    }

	internal void StopCollision()
	{
		isOnCollision = false;
	}

	internal void SideCollision()
    {
		isOnCollision = true;

		if (!kickTracker.IsOnKick)
			sideSpeed = -(sideSpeed.Value);
    }
}