using System.Collections.Generic;
using UnityEngine;

public class WorldMotionController : MonoBehaviour
{
    public float Speed = 1f;
    private HashSet<Transform> movingObjects = new HashSet<Transform>();

    private void Update()
    {
        Vector3 displacemnt = Speed * (-Time.deltaTime) * Vector3.forward;
        HashSet<Transform>.Enumerator enumerator = movingObjects.GetEnumerator();
        while (enumerator.MoveNext())
        {
            Transform transform = enumerator.Current.transform;
            transform.position += displacemnt;
        }
        enumerator.Dispose();
    }

    public void Add(Transform transform)
    {
        movingObjects.Add(transform);
    }

    public void Remove(Transform transform)
    {
        movingObjects.Remove(transform);
    }
}