using System.Collections.Generic;
using UnityEngine;

public class BaseObjectPool : IReleaser
{
	public readonly Transform InitialObjectTransform;
	private Transform _parentTransform;
	private Stack<Transform> notUsedObjects = new Stack<Transform>();

	public BaseObjectPool(Transform initialObjectTransform, Transform parentTransform)
	{
		InitialObjectTransform = initialObjectTransform;
		_parentTransform = parentTransform;
		InitialObjectTransform.gameObject.SetActive(false);
	}

	public virtual Transform Engage(Vector3 position)
	{
		Transform newObjectTransform;
		if (notUsedObjects.Count == 0)
		{
			newObjectTransform = GameObject.Instantiate(InitialObjectTransform, _parentTransform);
		}
		else
		{
			newObjectTransform = notUsedObjects.Pop();
		}
		newObjectTransform.gameObject.SetActive(true);
		newObjectTransform.position = position;
		return newObjectTransform;
	}

	public void Release(Transform obj)
	{
		obj.gameObject.SetActive(false);
		notUsedObjects.Push(obj);
	}
}