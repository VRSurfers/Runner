using System.Collections.Generic;
using UnityEngine;

public class TrackObjectsManager : MonoBehaviour
{
	class SeriesInfo : IReleaser
	{
		public SeriesInfo NextSeries;
		public PoolingPair? Kit;
		public readonly PoolingPair?[] Cars;
		public readonly Transform SeriesTransform;
		private readonly TrackObjectsManager trackObjectsManager;

		public SeriesInfo(int rowsCount, TrackObjectsManager trackObjectsManager, Transform seriesTransform)
		{
			SeriesTransform = seriesTransform;
			this.trackObjectsManager = trackObjectsManager;
			Cars = new PoolingPair?[rowsCount];
		}

		public void Release(Transform objTransform)
		{
			trackObjectsManager.Release(objTransform, this);
		}
	}

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

	public MapController MapController;
	public WorldMotionController WorldMotionController;
	public Collider SeriesObjectCollider;
	public GameObject[] InitialCars;
	public GameObject[] InitialKits;
	public float KitsProbability = 0.5f;

	private readonly Dictionary<Transform, SeriesInfo> seriesToSeriesInfo = new Dictionary<Transform, SeriesInfo>();

	private BaseObjectPool[] carPools;
	private BaseObjectPool[] kitsPools;
	private BaseObjectPool seriesPool;
	private SeriesInfo lastSeries;

	private void Awake()
	{		
		seriesPool = new BaseObjectPool(SeriesObjectCollider.transform, transform);
		carPools = CreatePool(InitialCars, transform);
		kitsPools = CreatePool(InitialKits, transform);
		InitDefaultTrack();
	}

	internal float GetNextFreeX(Transform seriesTransform)
	{
		SeriesInfo nextSeries = seriesToSeriesInfo[seriesTransform].NextSeries;
		PoolingPair?[] cars = nextSeries.Cars;
		for (int i = 0; i < cars.Length; i++)
		{
			if (!cars[i].HasValue)
				return MapController.LeftRowX + i * MapController.RowWidth;
		}
		throw new System.InvalidOperationException();
	}

	public void ReleaseKit(Transform seriesTransform)
	{
		SeriesInfo seriesinfo = seriesToSeriesInfo[seriesTransform];
		seriesinfo.Kit.Value.Release();
		seriesinfo.Kit = null;
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
		PoolingPair?[] cars = seriesInfo.Cars;
		for (int i = 0; i < cars.Length; i++)
		{
			PoolingPair? car = cars[i];
			if (car.HasValue)
			{
				PoolingPair carValue = car.Value;
				carValue.Release();
				carValue.PooledObject.transform.SetParent(null);
			}
		}
		if (seriesInfo.Kit.HasValue)
		{
			seriesInfo.Kit.Value.Release();
			seriesInfo.Kit.Value.PooledObject.transform.SetParent(null);
			seriesInfo.Kit = null;
		}
		seriesToSeriesInfo.Remove(seriesTransform);
		PushToTheEnd();
	}

	SeriesInfo CreateSeries(Vector3 seriesPosition)
	{
		Transform newSeriesTransform = seriesPool.Engage(seriesPosition);
		var seriesInfo = new SeriesInfo(MapController.RowCount, this, newSeriesTransform);
		AddCarsToSeries(seriesInfo);
		AddKitsToSeries(seriesInfo);
		seriesToSeriesInfo.Add(newSeriesTransform, seriesInfo);
		WorldMotionController.Add(newSeriesTransform, seriesInfo);
		return seriesInfo;
	}

	private void AddCarsToSeries(SeriesInfo seriesInfo)
	{
		Transform seriesTransform = seriesInfo.SeriesTransform;
		int randomFreeCell = MapController.GetRandomTrackNumber();
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
				carTransform.localPosition = new Vector3(MapController.GetTrackX(i), carPool.InitialObjectTransform.position.y, 0);
				cars[i] = new PoolingPair(carTransform, carPool);
			}
		}
	}

	private void AddKitsToSeries(SeriesInfo seriesInfo)
	{
		if (Random.value < KitsProbability)
		{
			BaseObjectPool randomPool = kitsPools[Random.Range(0, kitsPools.Length)];
			Transform kit = randomPool.Engage(new Vector3());
			kit.SetParent(seriesInfo.SeriesTransform);
			kit.localPosition = new Vector3(MapController.GetTrackX(MapController.GetRandomTrackNumber()), 0.5f, MapController.DistanceByZ * 0.4f);
			seriesInfo.Kit = new PoolingPair(kit, randomPool);
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