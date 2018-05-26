using System;
using UnityEngine;

public partial class RunnerController : MonoBehaviour
{
	public Transform RunnerModel;
	public MapController MapController;
	public float MaxSideSpeed = 2;

	private KickTracker kickTracker;
    private float? sideSpeed;

	private void Awake()
	{
		kickTracker = new KickTracker(RunnerModel);
	}

	void Update()
	{
		float? horizontal;
		float? vertical;
		InputController.GetmovementInfo(out horizontal, out vertical);
		if (horizontal.HasValue && !kickTracker.IsOnKick)
		{
            TryStartSideMotion(horizontal.Value);
		}

        if (kickTracker.IsOnKick)
			kickTracker.Step();
        else
            MakeMoveAndTryStopSideMotion();
	}

    private void TryStartSideMotion(float newSideSpeedSign)
    {
        if (sideSpeed.HasValue)
        {
            sideSpeed = newSideSpeedSign * MaxSideSpeed;
            return;
        }

        // We are on track.
        float xCurrent = transform.position.x;
        float relativePosition = (xCurrent - MapController.LeftRowX) / MapController.RowWidth;

		if (Math.Abs(relativePosition - Math.Round(relativePosition)) > 0.001)
			1.ToString();

		// teorically it should has int value.
		//CommonHelpers.AssertIfTrue(Math.Abs(relativePosition / 1f) > 0.001);

		int trackNumber = (int)Math.Round(relativePosition);
        // check if we are on outside tracks
        if (newSideSpeedSign < 0)
        {
            if (trackNumber == 0)
                return;
        }
        else
        {
            if (trackNumber == MapController.RowCount - 1)
                return;
        }
        sideSpeed = newSideSpeedSign * MaxSideSpeed;
    }

	private void MakeMoveAndTryStopSideMotion()
	{
		if (sideSpeed.HasValue)
		{
			float leftRowX = MapController.LeftRowX;
			float rowWidth = MapController.RowWidth;

			float displacement;
			Vector3 oldPositon = transform.position;
			float xCurrent = oldPositon.x;
			float maxDisplAbs = Time.deltaTime * MaxSideSpeed;

			double xRelative = (xCurrent - leftRowX) / rowWidth;
			if (Math.Abs(xRelative - Math.Round(xRelative)) < 0.00001) // we are starting motion
			{
				displacement = maxDisplAbs * Math.Sign(sideSpeed.Value);
			}
			else
			{
				float betweanTracksX = (xCurrent - leftRowX) % rowWidth;
				float toTrackDist = sideSpeed.Value > 0 ? rowWidth - betweanTracksX : betweanTracksX; // >= 0
				if (toTrackDist < maxDisplAbs)
				{
					displacement = toTrackDist * Math.Sign(sideSpeed.Value);
					sideSpeed = null;
				}
				else
				{
					displacement = maxDisplAbs * Math.Sign(sideSpeed.Value);
				}
			}
			transform.position = oldPositon + new Vector3(displacement, 0, 0);
		}
	}

	internal void FrontCollision(float x)
    {
		kickTracker.KickTo(x);
        sideSpeed = null;
    }

    internal void SideCollision()
    {
        sideSpeed = -(sideSpeed.Value);
    }
}