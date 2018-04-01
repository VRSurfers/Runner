using System.Collections.Generic;
using UnityEngine;

public class DronFactory : MonoBehaviour {

	public GameObject Target;
	public DroneScript DronExample;
	public ExplosionFactory ExplosionFactory;

	private Stack<DroneScript> pool = new Stack<DroneScript>();
	private const float Interval = 3;
	private float lastActivationTime;

	// Use this for initialization
	void Start () {
		DronExample.gameObject.SetActive(false);
		DronExample.Target = Target;
		lastActivationTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > lastActivationTime + Interval)
		{
			DroneScript newOne = EngageDron();
			newOne.transform.position =
				Target.transform.position
				+ 20 * Target.transform.forward
				+ 2 * Mathf.Sign(Random.value - 0.5f) * Target.transform.right
				+ 4 * Vector3.up;
			newOne.gameObject.SetActive(true);
			lastActivationTime = Time.time;
		}
	}

	DroneScript EngageDron()
	{
		DroneScript newDrone;
		if (pool.Count == 0)
			newDrone = Instantiate(DronExample, transform);
		else
			newDrone = pool.Pop();
		return newDrone;
	}

	public void FreeDrone(DroneScript droneScript, bool withExplosion)
	{
		if (withExplosion)
			ExplosionFactory.Boom(droneScript.transform);
		droneScript.gameObject.SetActive(false);
		pool.Push(droneScript);
	}
}
