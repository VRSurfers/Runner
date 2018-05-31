using UnityEngine;

public class DronManager : MonoBehaviour
{
	public RunnerController Target;
	public MapController MapController;
	public WorldMotionController WorldMotionController;
	public Transform InitialDrone;
	public Transform InitialEplosion;

	private const float Interval = 5;
	private BaseObjectPool dronePool;
	private BaseObjectPool explosionPool;
	private float lastActivationTime;

	void Start ()
	{
		lastActivationTime = Time.time;
		dronePool = new BaseObjectPool(InitialDrone, transform);
		explosionPool = new BaseObjectPool(InitialEplosion, transform);
	}
	
	void Update ()
	{
		if (Time.time > lastActivationTime + Interval)
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
		float x;
		if (Random.Range(0, 2) == 1)
			x = MapController.LeftRowX;
		else
			x = MapController.GetRghtX();
		Transform drone = dronePool.Engage(new Vector3(x, 0, 30));
		WorldMotionController.Add(drone, new DroneController(drone, Target, dronePool));
	}
}

internal class DroneController : IReleaserUpdater
{
	public float HangingHeight = 2;
	public float VerticalSpeed = 2;
	public float ShootingDeltaTime = 1f;
	public float DistanceOfDaamage = 20f;
	public float DamegePerShot = 1f;

	private readonly Transform transform;
	private readonly RunnerController target;
	private readonly BaseObjectPool baseObjectPool;
	private Health health;
	private float lastShootTime;

	public DroneController(Transform drone, RunnerController runnerController, BaseObjectPool baseObjectPool)
	{
		transform = drone;
		target = runnerController;
		this.baseObjectPool = baseObjectPool;
		health = target.GetComponent<Health>();
	}

	public void Return(Transform objTransform)
	{
		baseObjectPool.Release(objTransform);
	}

	public void Update()
	{
		CorrectHeight();
		CorrectRotation();
		TryDamageTarget();
	}

	void CorrectHeight()
	{
		Vector3 position = transform.position;
		if (HangingHeight != position.y)
		{
			float delta = HangingHeight - transform.position.y;
			position.y += Mathf.Sign(delta) * Mathf.Min(VerticalSpeed * Time.deltaTime, Mathf.Abs(delta));
			transform.position = position;
		}
	}

	void CorrectRotation()
	{
		transform.rotation = Quaternion.Euler(-90, 0, GetZRotOnTarget());
	}

	float GetZRotOnTarget()
	{
		Vector3 targetDirection = target.transform.position - transform.position;
		targetDirection.y = 0;

		float a1 = Vector3.Angle(Vector3.forward, targetDirection);

		float a2 = Vector3.Angle(Vector3.right, targetDirection);
		a2 = a2 - 90 <= 0 ? +1 : -1;
		return a2 * a1;
	}

	private void TryDamageTarget()
	{
		float currentTime = Time.time;
		if (currentTime - lastShootTime > ShootingDeltaTime)
		{
			if ((transform.position - target.transform.position).magnitude < DistanceOfDaamage)
			{
				lastShootTime = currentTime;
				health.HP -= DamegePerShot;
			}
		}
	}
}