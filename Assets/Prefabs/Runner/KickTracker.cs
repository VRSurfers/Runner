using UnityEngine;

public partial class RunerMotionController
{
	class KickTracker
    {
		private const float FlyTime = 1.2f;
		private const float AngleSpeed = 360 / FlyTime;
		private const float Axeleration = 10f;
		private const float LandingHeight = 0f;

		public bool IsOnKick { get; private set; }

		private readonly Transform transformWithCamera;
		private readonly Transform transformOfModel;

		private float targetX;
		private float startTime;
		private float horizontalSpeed;
		private float verticalSpeed;

		public KickTracker(Transform transformOfModel)
		{
			this.transformOfModel = transformOfModel;
			transformWithCamera = transformOfModel.parent.transform;
		}

		public void KickTo(float targetX)
		{
			this.targetX = targetX;
			startTime = Time.time;

			horizontalSpeed = (targetX - transformWithCamera.position.x) / FlyTime;
			verticalSpeed = Axeleration * FlyTime / 2f;
			IsOnKick = true;
		}

        public void Step()
        {
			CommonHelpers.AssertIfTrue(IsOnKick);

            Vector3 oldPosition = transformWithCamera.position;
            float timePassed = (Time.time - startTime);
            if (timePassed > FlyTime)
			{
				oldPosition.x += targetX - oldPosition.x;
				oldPosition.y = LandingHeight;
				transformOfModel.localRotation = Quaternion.Euler(0, 0, 0);
				IsOnKick = false;
			}
			else
			{
				oldPosition.x += horizontalSpeed * Time.deltaTime;
				oldPosition.y = timePassed * (verticalSpeed - Axeleration * timePassed / 2f);
				transformOfModel.localRotation = Quaternion.Euler(AngleSpeed * timePassed, 0, 0);
			}
			transformWithCamera.position = oldPosition;
		}
    }
}