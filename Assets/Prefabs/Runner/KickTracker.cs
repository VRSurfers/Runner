using UnityEngine;

public partial class RunerMotionController
{
	class KickTracker
    {
		private const float KickFullFlyTime = 1.2f;
		private const float JumpFlyTime = 1.3f;
		private float AngleSpeed;
		//private const float AngleSpeed = 360 / FlyTime;
		private const float Axeleration = -10f;

		public FlyType FlyType { get; private set; }

		private readonly Transform transformWithCamera;
		private readonly Transform transformOfModel;
		private readonly float floor;

		private float targetX;
		private float horizontalSpeed;
		private float startY;
		private float startTime;
		private float V0;
		private float flyTime;
		private float angle = 0;

		public KickTracker(Transform transformOfModel)
		{
			this.transformOfModel = transformOfModel;
			transformWithCamera = transformOfModel.parent.transform;
			floor = transformWithCamera.transform.position.y;
		}

		public void KickByCar(float newX)
		{
			if (FlyType == FlyType.None)
			{
				angle = 0;
				V0 = -Axeleration * KickFullFlyTime / 2;
				//
				flyTime = KickFullFlyTime;
			}
			else if (FlyType == FlyType.Jump)
			{
				float curV = V0 + (Time.time - startTime) * Axeleration;
				if (curV < 0)
				{
					V0 = -curV;
					flyTime = Time.time - startTime;
				}
				else
				{
					//float timePassed = Time.time - startTime;
					V0 = curV;
					//float defaultKickSpeed = -Axeleration * KickFullFlyTime / 2;
					//float timePassed = (curV - defaultKickSpeed) / Axeleration;
					flyTime = JumpFlyTime - (Time.time - startTime);
				}
			}


			AngleSpeed = 360 / KickFullFlyTime;
			startTime = Time.time;
			targetX = newX;
			startY = transformWithCamera.position.y;

			FlyType = FlyType.AfterCash;
			//flyTime = Qwe();
			horizontalSpeed = (targetX - transformWithCamera.position.x) / flyTime;
		}

		public void Jump()
		{
			angle = 0;
			AngleSpeed = 360 / JumpFlyTime;
			V0 = -Axeleration * JumpFlyTime / 2;
			startTime = Time.time;
			targetX = transformWithCamera.position.x;
			startY = transformWithCamera.position.y;

			FlyType = FlyType.Jump;
			flyTime = Qwe();
			horizontalSpeed = 0;
		}

		float Qwe()
		{
			float h0 = (transformWithCamera.transform.position.y - floor);
			float D = Mathf.Sqrt(V0 * V0 - Axeleration * h0 / 2);
			return (-V0 - D) / Axeleration;
		}


		public void Step()
        {
			Vector3 eulerRotaion = transformOfModel.localRotation.eulerAngles;

            Vector3 oldPosition = transformWithCamera.position;
            float timePassed = (Time.time - startTime);

            if (timePassed > flyTime)
			{
				oldPosition.x += targetX - oldPosition.x;
				oldPosition.y = floor;
				//transformOfModel.localRotation = Quaternion.Euler(0, 0, 0);
				FlyType = FlyType.None;
			}
			else
			{
				oldPosition.x += horizontalSpeed * Time.deltaTime;
				oldPosition.y = startY + V0 * timePassed + Axeleration * timePassed * timePassed / 2f;
				//transformOfModel.localRotation = Quaternion.Euler(AngleSpeed * timePassed, 0, 0);
			}
			angle = angle + AngleSpeed * Time.deltaTime;
			transformOfModel.localRotation = Quaternion.Euler(angle >= 360 ? 0 : angle, 0, 0);
			transformWithCamera.position = oldPosition;
		}
    }

	enum FlyType
	{
		None = 0,
		AfterCash,
		Jump
	}
}