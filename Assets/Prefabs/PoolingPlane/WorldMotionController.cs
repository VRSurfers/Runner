using System.Collections.Generic;
using UnityEngine;


public class WorldMotionController : MonoBehaviour
{
	public float Speed = 10f;

	private Dictionary<Transform, IReleaser> movingObjects = new Dictionary<Transform, IReleaser>();
	private Dictionary<Transform, IReleaserUpdater> updatingObjects = new Dictionary<Transform, IReleaserUpdater>();

	private void Update()
    {
        Vector3 displacemnt = Speed * (-Time.deltaTime) * Vector3.forward;
		Dictionary<Transform, IReleaser>.KeyCollection.Enumerator keysEnumerator = movingObjects.Keys.GetEnumerator();
        while (keysEnumerator.MoveNext())
        {
            Transform transform = keysEnumerator.Current.transform;
            transform.position += displacemnt;
        }

		Dictionary<Transform, IReleaserUpdater>.Enumerator enumerator = updatingObjects.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<Transform, IReleaserUpdater> pair = enumerator.Current;
			pair.Value.UpdateManual(pair.Key);
		}
    }

    public void Add(Transform obj, IReleaser releaser)
    {
        movingObjects.Add(obj, releaser);
    }

	public void Add(Transform obj, IReleaserUpdater releaserUpdater)
	{
		movingObjects.Add(obj, releaserUpdater);
		updatingObjects.Add(obj, releaserUpdater);
	}

	public void Release(Transform obj)
	{
		IReleaser releaser = movingObjects[obj];
		movingObjects.Remove(obj);
		updatingObjects.Remove(obj);
		releaser.Release(obj);
	}

	private void OnTriggerEnter(Collider other)
	{
		Release(other.transform);
	}
}

public interface IReleaser
{
	void Release(Transform objTransform);
}

public interface IReleaserUpdater : IReleaser
{
	void UpdateManual(Transform objectForUpdate);
}