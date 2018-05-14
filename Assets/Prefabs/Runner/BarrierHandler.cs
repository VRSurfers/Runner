using UnityEngine;

public class BarrierHandler : MonoBehaviour {
    public float Size = 0.95f;
    public RunnerController Runner;

    private void OnTriggerEnter(Collider other)
    {
        if (Mathf.Abs(Runner.transform.position.x - other.transform.position.x) < Size)
        {
            var row = other.transform.parent.GetComponent<CarRowComponent>();
            float x = row.NextRow.GetFreeX();
            Runner.FrontCollision(x);
        }
        else
        {
            Runner.SideCollision();
        }
    }
}
