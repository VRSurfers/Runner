using UnityEngine;

public class DronFactory : MonoBehaviour {

	public GameObject Target;
	public DroneScript Dron;

	private const float Interval = 1;
	private float lastActivationTime;

	// Use this for initialization
	void Start () {
		Dron.gameObject.SetActive(false);
		Dron.Target = Target;
		lastActivationTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > lastActivationTime + Interval)
		{
			Dron.transform.position = Target.transform.position + 20 * Target.transform.forward;
			Dron.gameObject.SetActive(true);
			lastActivationTime = Time.time + 1000;
		}
	}
}
