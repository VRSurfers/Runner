using UnityEngine;

public class Stoper : MonoBehaviour {

	public RunnerController Runner;

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("OnTriggerEnter with: " + other.gameObject.name);
		Runner.StopSideMotion(-other.transform.right);
		TrackKeeper trackKeeper = Runner.TrackKeeper;
		if (trackKeeper.Track != other.gameObject && trackKeeper.Track != null)
			trackKeeper.Track.SetActive(false);
		trackKeeper.Track = other.gameObject;
	}

	void OnTriggerExit(Collider other)
	{
		Debug.Log("OnTriggerEnter with: " + other.gameObject.name);
		Runner.TrackKeeper.Track = null;
	}
}
