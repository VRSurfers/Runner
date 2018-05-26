using System;
using UnityEngine;

public class BarrierHandler : MonoBehaviour {
    public float Size = 0.95f;
    public RunnerController Runner;

    private void OnTriggerEnter(Collider other)
    {
		Vector3 runnerPosition = Runner.transform.position;
		Vector3 carPosition = other.transform.position;

		if (Math.Abs(runnerPosition.x - carPosition.x) < Size)
        {
			CarRowComponent row = other.transform.parent.GetComponent<CarRowComponent>();
            Runner.FrontCollision(row.NextRow.GetFreeX());
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
