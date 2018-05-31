using System;
using UnityEngine;

public class BarrierHandler : MonoBehaviour
{
    public float Size = 0.95f;
    public RunnerController Runner;
	public TrackObjectsManager TrackObjectsManager;

    private void OnTriggerEnter(Collider other)
    {
		Vector3 runnerPosition = Runner.transform.position;
		Vector3 carPosition = other.transform.position;

		if (Math.Abs(runnerPosition.x - carPosition.x) < Size)
        {
            Runner.FrontCollision(TrackObjectsManager.GetNextFreeX(other.transform));
        }
        else
        {
            Runner.SideCollision();
        }
    }

	private void OnTriggerExit(Collider other)
	{
		Runner.StopCollision();
	}
}
