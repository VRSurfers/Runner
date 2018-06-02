using UnityEngine;

public class DronManager : MonoBehaviour, IReleaserUpdater
{
	public float HangingHeight = 2;
	public float VerticalSpeed = 2;
	public float ShootingDeltaTime = 1f;
	public float DistanceOfDaamage = 20f;
	public float DamegePerShot = 2f;
	public float ProducingInterval = 5;

	public RunerMotionController Target;
	public MapController MapController;
	public WorldMotionController WorldMotionController;
	public Transform InitialDrone;
	public Explosion InitialEplosion;

	private BaseObjectPool dronePool;
	private BaseObjectPool explosionPool;
	private float lastActivationTime;
	private float lastShootTime;

	void Start ()
	{
		InitialEplosion.WorldMotionController = WorldMotionController;
		lastActivationTime = Time.time;
		dronePool = new BaseObjectPool(InitialDrone, transform);
		explosionPool = new BaseObjectPool(InitialEplosion.transform, transform);
	}
	
	void Update ()
	{
		if (Time.time > lastActivationTime + ProducingInterval)
		{
			ProduceDrone();
			lastActivationTime = Time.time;
		}
	}

	public void DestroyDrone(Transform droneTransform)
	{
		WorldMotionController.Release(droneTransform);
		dronePool.Release(droneTransform);
		Transform explosion = explosionPool.Engage(droneTransform.position);
		WorldMotionController.Add(explosion, explosionPool);
	}

	private void ProduceDrone()
	{
		const float offset = 1f;
		float x;
		if (Random.Range(0, 2) == 1)
			x = MapController.LeftRowX - offset;
		else
			x = MapController.GetRghtX() + offset;
		Transform drone = dronePool.Engage(new Vector3(x, 0, 30));
		WorldMotionController.Add(drone, this);
	}

	void IReleaserUpdater.UpdateManual(Transform objectForUpdate)
	{
		CorrectHeight(objectForUpdate);
		CorrectRotation(objectForUpdate);
		TryDamageTarget(objectForUpdate);
	}

	void IReleaser.Release(Transform objTransform)
	{
		dronePool.Release(objTransform);
	}

	void CorrectHeight(Transform droneTransform)
	{
		Vector3 position = droneTransform.position;
		if (HangingHeight != position.y)
		{
			float delta = HangingHeight - droneTransform.position.y;
			position.y += Mathf.Sign(delta) * Mathf.Min(VerticalSpeed * Time.deltaTime, Mathf.Abs(delta));
			droneTransform.position = position;
		}
	}

	void CorrectRotation(Transform droneTransform)
	{
		Vector3 targetDirection = Target.transform.position - droneTransform.position;
		targetDirection.y = 0;

		float a1 = Vector3.Angle(Vector3.forward, targetDirection);

		float a2 = Vector3.Angle(Vector3.right, targetDirection);
		a2 = a2 - 90 <= 0 ? +1 : -1;
		droneTransform.rotation = Quaternion.Euler(-90, 0, a2 * a1);
	}

	private void TryDamageTarget(Transform droneTransform)
	{
		float currentTime = Time.time;
		if (currentTime - lastShootTime > ShootingDeltaTime)
		{
			if ((droneTransform.position - Target.transform.position).magnitude < DistanceOfDaamage)
			{
				lastShootTime = currentTime;
				Target.Health.Change(-DamegePerShot);
			}
		}
	}
}