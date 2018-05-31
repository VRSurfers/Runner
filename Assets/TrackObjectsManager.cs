using System.Collections.Generic;
using UnityEngine;

struct PoolingPair
{
	public readonly Transform PooledObject;
	private readonly BaseObjectPool _pool;

	public PoolingPair(Transform pooledObject, BaseObjectPool pool)
	{
		PooledObject = pooledObject;
		_pool = pool;
	}

	internal void Release()
	{
		_pool.Release(PooledObject);
	}
}

public class TrackObjectsManager : MonoBehaviour
{
	class SeriesInfo : IReleaser
	{
		public SeriesInfo NextSeries;
		public readonly PoolingPair?[] Cars;
		public readonly Transform SeriesTransform;
		private readonly TrackObjectsManager trackObjectsManager;

		public SeriesInfo(int rowsCount, TrackObjectsManager trackObjectsManager, Transform seriesTransform)
		{
			SeriesTransform = seriesTransform;
			this.trackObjectsManager = trackObjectsManager;
			Cars = new PoolingPair?[rowsCount];
		}

		public void Return(Transform objTransform)
		{
			trackObjectsManager.Release(objTransform, this);
		}
	}

	public MapController MapController;
	public WorldMotionController WorldMotionController;
	public Collider SeriesObjectCollider;
	public GameObject[] InitialCars;
	public GameObject[] InitialKits;

	private readonly Dictionary<Transform, SeriesInfo> carsToSeries = new Dictionary<Transform, SeriesInfo>();

	private BaseObjectPool[] carPools;
	private BaseObjectPool seriesPool;
	private SeriesInfo lastSeries;

	private void Awake()
	{
		
		seriesPool = new BaseObjectPool(SeriesObjectCollider.transform, transform);
		carPools = CreatePool(InitialCars, transform);
		InitDefaultTrack();
	}

	internal float GetNextFreeX(Transform transform)
	{
		SeriesInfo nextSeries = carsToSeries[transform].NextSeries;
		PoolingPair?[] cars = nextSeries.Cars;
		for (int i = 0; i < cars.Length; i++)
		{
			if (!cars[i].HasValue)
				return MapController.LeftRowX + i * MapController.RowWidth;
		}
		throw new System.InvalidOperationException();
	}

	private BaseObjectPool[] CreatePool(GameObject[] initialObjects, Transform parentTransform)
	{
		var newPools = new BaseObjectPool[initialObjects.Length];
		for (int i = 0; i < initialObjects.Length; i++)
		{
			GameObject initialObject = initialObjects[i];
			newPools[i] = new BaseObjectPool(initialObject.transform, parentTransform);
		}
		return newPools;
	}

	private void Release(Transform seriesTransform, SeriesInfo seriesInfo)
	{
		seriesPool.Release(seriesTransform);
		var cars = seriesInfo.Cars;
		for (int i = 0; i < cars.Length; i++)
		{
			PoolingPair? car = cars[i];
			if (car.HasValue)
			{
				PoolingPair carValue = car.Value;
				carValue.Release();
				carsToSeries.Remove(carValue.PooledObject);
				carValue.PooledObject.transform.SetParent(null);
			}
		}
		PushToTheEnd();
	}

	SeriesInfo CreateSeries(Vector3 seriesPosition)
	{
		Transform newSeriesTransform = seriesPool.Engage(seriesPosition);
		var seriesInfo = new SeriesInfo(MapController.RowCount, this, newSeriesTransform);
		AddCarsToSeries(seriesInfo);
		WorldMotionController.Add(newSeriesTransform, seriesInfo);
		return seriesInfo;
	}

	private void AddCarsToSeries(SeriesInfo seriesInfo)
	{
		Transform seriesTransform = seriesInfo.SeriesTransform;
		int randomFreeCell = Random.Range(0, MapController.RowCount);
		PoolingPair?[] cars = seriesInfo.Cars;
		for (int i = 0; i < cars.Length; i++)
		{
			if (i == randomFreeCell)
			{
				cars[i] = null;
			}
			else
			{
				int randomPoolNumber = Random.Range(0, carPools.Length);
				BaseObjectPool carPool = carPools[randomPoolNumber];
				Transform carTransform = carPool.Engage(new Vector3());
				carTransform.SetParent(seriesTransform);
				carTransform.localPosition = new Vector3(MapController.LeftRowX + i * MapController.RowWidth, carPool.InitialObjectTransform.position.y, 0);
				cars[i] = new PoolingPair(carTransform, carPool);
				carsToSeries.Add(carTransform, seriesInfo);
			}
		}
	}

	private void InitDefaultTrack()
	{
		lastSeries = CreateSeries(MapController.DistanceByZ * Vector3.forward);
		for (int i = 0; i < MapController.CarSeriesCount; i++)
		{
			PushToTheEnd();
		}
	}

	private void PushToTheEnd()
	{
		SeriesInfo newSeries = CreateSeries(
			lastSeries.SeriesTransform.position + MapController.DistanceByZ * Vector3.forward);
		lastSeries.NextSeries = newSeries;
		lastSeries = newSeries;
	}
}