using UnityEngine;

public class Stoper : MonoBehaviour {

	public RunnerController Runner;

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log("OnTriggerEnter: " + other.gameObject.name + " + " + name);
		Runner.StopSideMotion(-other.transform.right);
	}

	void OnTriggerExit(Collider other)
	{
		//Debug.Log("OnTriggerEnter: " + other.gameObject.name + " + " + name);
	}
}
