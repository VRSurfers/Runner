using UnityEngine;

public class DronManager : PoolWithMoving<DroneScript>
{
	public RunnerController Target;
	public ExplosionFactory ExplosionFactory;

	private const float Interval = 5;
	private float lastActivationTime;

	void Start () {
		lastActivationTime = Time.time;
	}
	
	void Update () {
		if (Time.time > lastActivationTime + Interval)
		{
			ProduceDrone();
			lastActivationTime = Time.time;
		}
	}

	private void ProduceDrone()
	{
        Vector3 position = Target.transform.position
            + 20 * Target.transform.forward
            + 3 * Mathf.Sign(Random.value - 0.5f) * Target.transform.right
            + 0 * Vector3.up;
        EngageOne(position);
	}
}
