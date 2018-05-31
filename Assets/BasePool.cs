using System;
using System.Collections.Generic;
using UnityEngine;

public class BasePoolRelease : MonoBehaviour
{
    public virtual void FreeByObj(GameObject gameObject)
    {

    }
}

public class TransformPool : BasePool<Transform> { }

public class BasePool<T> : BasePoolRelease
    where T : Component
{
    [NonSerialized]
    public T InitialComponent;
    protected Dictionary<GameObject, T> DictionaryOfObjectsInUse = new Dictionary<GameObject, T>();
    private Stack<T> notUsedObjects = new Stack<T>();

    private void Awake()
    {
        Awaiking();
    }

    public virtual void Awaiking()
    {
		if (InitialComponent != null)
			InitialComponent.gameObject.SetActive(false);
    }

    public virtual T EngageOne(Vector3 position)
    {
        T newObject;
        if (notUsedObjects.Count == 0)
        {
            newObject = Instantiate(InitialComponent, transform);
        }
        else
        {
            newObject = notUsedObjects.Pop();
        }
        newObject.gameObject.SetActive(true);
        newObject.transform.position = position;
        DictionaryOfObjectsInUse.Add(newObject.gameObject, newObject);
        return newObject;
    }

    public virtual void Free(T component)
    {
        var go = component.gameObject;
        go.SetActive(false);
        DictionaryOfObjectsInUse.Remove(go);
        notUsedObjects.Push(component);
    }

    public override void FreeByObj(GameObject gameObject)
    {
        Free(DictionaryOfObjectsInUse[gameObject]);
    }
}


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

	public virtual void Release(Transform obj)
	{
		obj.gameObject.SetActive(false);
		notUsedObjects.Push(obj);
	}

	public void Return(Transform objTransform)
	{
		Release(objTransform);
	}
}