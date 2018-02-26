using UnityEngine;

public class Stoper : MonoBehaviour {

	public RunnerController Runner;

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("OnTriggerEnter " + other.ToString());
		Runner.StopSideMotion(-other.transform.right);
	}
}
