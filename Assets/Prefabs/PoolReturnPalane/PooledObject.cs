using UnityEngine;

public class PooledObject : MonoBehaviour {

	[HideInInspector]
	[SerializeField]
	private BasePoolRelease ParentPool;

	public void BindToParent(BasePoolRelease basePoolRelease)
	{
		ParentPool = basePoolRelease;
	}

	private void OnTriggerEnter(Collider other)
    {
        ReturnToPool();
    }

    public virtual void ReturnToPool()
    {
        ParentPool.FreeByObj(gameObject);
    }
}
