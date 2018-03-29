using System.Collections.Generic;
using UnityEngine;

public class DronFactory : MonoBehaviour {

	public GameObject Target;
	public DroneScript DronExample;

	private List<DroneScript> pool = new List<DroneScript>();
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
			Debug.Log(newOne.name);
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
		if (pool.Count == 0)
		{
			DroneScript newDrone = Instantiate(DronExample);
			newDrone.Owner = this;
			return newDrone;
		}
		else
		{
			var newOne = pool[pool.Count - 1];
			pool.RemoveAt(pool.Count - 1);
			return newOne;
		}
	}

	public void FreeDrone(DroneScript droneScript)
	{
		droneScript.gameObject.SetActive(false);
		pool.Add(droneScript);
	}
}
