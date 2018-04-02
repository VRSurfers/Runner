using UnityEngine;

public class BarrierHandler : MonoBehaviour {

    public RunnerController Runner;

    private void OnTriggerEnter(Collider other)
    {
        Runner.CollideWithBarier();
    }

	private void OnTriggerExit(Collider other)
	{
        Runner.StopCollision();
	}
}
