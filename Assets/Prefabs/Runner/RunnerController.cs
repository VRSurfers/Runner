using System;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
    struct KickTracker
    {
        private const float FlyTime = 1.5f;
        private readonly Transform transform;
        private readonly float targetX;
        float startTime;
        private float G;
        private float V0;

        private float dAngle;
        private float horSpeedAbs;
        private float horSpeedSign;

        public KickTracker(Transform transform, float targetX)
        {
            this.transform = transform;
            this.targetX = targetX;
            startTime = Time.time;


            horSpeedAbs = (targetX - transform.position.x) / FlyTime;
            horSpeedSign = Math.Sign(horSpeedAbs);
            horSpeedAbs *= horSpeedSign;

            dAngle = (float) (360 / FlyTime);

            G = 15f;
            // V0 = g * t / 2;
            V0 = G * FlyTime / 2f;
        }

        public bool Step()
        {
            Vector3 oldPos = transform.position;

            float passed = (Time.time - startTime);

            if (passed > FlyTime)
                return true;

            oldPos.x += Math.Min(horSpeedAbs * Time.deltaTime, (targetX - oldPos.x) * horSpeedSign) * horSpeedSign;

            oldPos.y = Math.Max(V0 * passed - G * passed * passed / 2f, 0.077f);


            transform.localRotation = Quaternion.Euler(dAngle * passed, 0, 0);
            transform.parent.transform.position = oldPos;

            return false;
        }
    }

    public MapController MapController;
    public float MaxSideSpeed = 2;

    private float? sideSpeed;

	void Update()
	{
		float? horizontal;
		float? vertical;
		InputController.GetmovementInfo(out horizontal, out vertical);
		if (horizontal.HasValue && kickTracker == null)
		{
            TryUpdateSideSpeed(horizontal.Value);
		}

        if (kickTracker.HasValue)
        {
            if (kickTracker.Value.Step())
                kickTracker = null;
        }
        else
            MakeMoveAndTryStopSideMotion();
	}

    private void TryUpdateSideSpeed(float newSideSpeed)
    {
        if (sideSpeed.HasValue)
        {
            sideSpeed = newSideSpeed;
            return;
        }

        // We are on track.
        float xCurrent = transform.position.x;
        float relativePosition = (xCurrent - MapController.LeftRowX) / MapController.RowWidth; // teorically it should has int value (almost).
        int trackNum = (int)Math.Round(relativePosition);
        // check if we are on outside tracks
        if (newSideSpeed < 0)
        {
            if (trackNum == 0)
                return;
        }
        else
        {
            if (trackNum == MapController.RowCount - 1)
                return;
        }
        sideSpeed = newSideSpeed;
    }

    private KickTracker? kickTracker;
    public Transform Capsule;


    internal void FrontCollision(float x)
    {
        //var oldPos = transform.position;
        //oldPos.x = x;
        //transform.position = oldPos;

        kickTracker = new KickTracker(Capsule, x);
        sideSpeed = null;
    }

    internal void SideCollision()
    {
        sideSpeed = -(sideSpeed.Value);
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
}