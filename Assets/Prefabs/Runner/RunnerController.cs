using System;
using UnityEngine;

public partial class RunnerController : MonoBehaviour
{
	public Transform RunnerModel;
	public MapController MapController;
	public float MaxSideSpeed = 2;

	private KickTracker kickTracker;
    private float? sideSpeed;
	private bool isOnCollision;
	private HealthComponent healthComponent;
	private AmoComponent amoComponent;

	public HealthComponent HealthComponent
	{
		get { return healthComponent; }
	}

	public AmoComponent AmoComponent
	{
		get { return amoComponent; }
	}

	private void Awake()
	{
		kickTracker = new KickTracker(RunnerModel);
		healthComponent = GetComponent<HealthComponent>();
		amoComponent = GetComponent<AmoComponent>();
	}

	void Update()
	{
		float? horizontal;
		float? vertical;
		InputController.GetmovementInfo(out horizontal, out vertical);

		if (kickTracker.IsOnKick)
		{
			kickTracker.Step();
			return;
		}

		if (isOnCollision)
			horizontal = null; // prevents moving into coolider after rebound

		ProcessMotion(horizontal);
	}

	private void ProcessMotion(float? newSideSpeedSign)
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