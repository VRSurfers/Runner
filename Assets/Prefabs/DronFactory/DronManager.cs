using System.Collections.Generic;
using UnityEngine;

public class DronManager : MonoBehaviour {

	public RunnerController Target;
	public DroneScript DronExample;
	public ExplosionFactory ExplosionFactory;

	private Stack<DroneScript> unactiveDrones = new Stack<DroneScript>();

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
			ProduceDrone();
			lastActivationTime = Time.time;
		}
	}

	private void ProduceDrone()
	{
		DroneScript newDrone;
		if (unactiveDrones.Count == 0)
			newDrone = Instantiate(DronExample, transform);
		else
			newDrone = unactiveDrones.Pop();

		newDrone.transform.position =
			Target.transform.position
			+ 20 * Target.transform.forward
			+ 3 * Mathf.Sign(Random.value - 0.5f) * Target.transform.right
			+ 0 * Vector3.up;
		newDrone.gameObject.SetActive(true);
	}

	public void FreeDrone(DroneScript droneScript, bool withExplosion)
	{
		if (withExplosion)
			ExplosionFactory.Boom(droneScript.transform);
		droneScript.gameObject.SetActive(false);
		unactiveDrones.Push(droneScript);
	}
}
