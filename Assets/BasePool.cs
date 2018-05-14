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

public class PoolWithMoving<T> : BasePool<T>
    where T : PooledObject
{
    public T InitialObject;
    public WorldMotionController WorldMotionController;

    public override void Awaiking()
    {
        InitialComponent = InitialObject;
		InitialObject.BindToParent(this);
        base.Awaiking();
    }

    public void AddExistingToPool(T poolObject)
    {
		poolObject.BindToParent(this);
        WorldMotionController.Add(poolObject.transform);
        DictionaryOfObjectsInUse.Add(poolObject.gameObject, poolObject);
    }

    public override T EngageOne(Vector3 position)
    {
        var newObe = base.EngageOne(position);
		newObe.BindToParent(this);
        WorldMotionController.Add(newObe.transform);
        return newObe;
    }

    public override void Free(T component)
    {
        base.Free(component);
        WorldMotionController.Remove(component.transform);
    }
}


public class ObjectPool<T> where T : Component
{
	private T _initialObject;
	private Transform _parentTransform;
	private Stack<T> notUsedObjects = new Stack<T>();

	public ObjectPool(T initialObject, Transform parentTransform)
	{
		_initialObject = initialObject;
		_parentTransform = parentTransform;
		_initialObject.gameObject.SetActive(false);
	}

	public virtual T EngageOne(Vector3 position)
	{
		T newObject;
		if (notUsedObjects.Count == 0)
		{
			newObject = GameObject.Instantiate(_initialObject, _parentTransform);
		}
		else
		{
			newObject = notUsedObjects.Pop();
		}
		newObject.gameObject.SetActive(true);
		newObject.transform.position = position;
		return newObject;
	}

	public virtual void Free(T component)
	{
		GameObject gameObject = component.gameObject;
		gameObject.SetActive(false);
		notUsedObjects.Push(component);
	}
}

public class MovingObjectPool<T> : ObjectPool<T> 	where T : Component
{
	private WorldMotionController _worldMotionController;

	public MovingObjectPool(T initialObject, Transform parentTransform, WorldMotionController worldMotionController)
		: base(initialObject, parentTransform)
	{
		_worldMotionController = worldMotionController;
	}

	public override T EngageOne(Vector3 position)
	{
		T newObject = base.EngageOne(position);
		_worldMotionController.Add(newObject.transform);
		return newObject;
	}

	public override void Free(T component)
	{
		base.Free(component);
		_worldMotionController.Remove(component.transform);
	}
}