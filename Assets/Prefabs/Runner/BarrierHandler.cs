using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHandler : MonoBehaviour {

    public RunnerController Runner;

    private void OnTriggerEnter(Collider other)
    {
        Runner.CollideWithBarier();
    }
}
