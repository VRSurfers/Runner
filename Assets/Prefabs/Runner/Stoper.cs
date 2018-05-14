using UnityEngine;

public class Stoper : MonoBehaviour {

	public RunnerController Runner;

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log("OnTriggerEnter: " + other.gameObject.name + " + " + name);
		//Runner.StopSideMotion(DirectionHelper.GetDirectionFromTrack(other.transform));
	}

	void OnTriggerExit(Collider other)
	{
		//Debug.Log("OnTriggerEnter: " + other.gameObject.name + " + " + name);
	}
}

static class DirectionHelper
{
	public static Vector3 GetDirectionFromTrack(Transform transform)
	{
		return -transform.right;
	}
}
