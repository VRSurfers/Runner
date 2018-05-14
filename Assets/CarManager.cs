using UnityEngine;

public class CarManager : PoolWithMoving<CarRowComponent>
{
    public MapController MapController;

	public GameObject[] InitialCars;

    private BasePool<Transform>[] pools;
    private CarRowComponent lastRow;
    private System.Random random = new System.Random();

    public override void Awaiking()
    {
        base.Awaiking();
        InitialObject.CarManager = this;
    }

	void Start()
    {
        pools = new BasePool<Transform>[InitialCars.Length];
        for (int i = 0; i < InitialCars.Length; i++)
        {
            GameObject car = InitialCars[i];
            var newPool = gameObject.AddComponent<TransformPool> ();
			car.SetActive(false);
			newPool.InitialComponent = car.transform;
            pools[i] = newPool;
        }
        InitRows();
    }

    private void InitRows()
    {
        lastRow = CraeteCarRow(MapController.DistanceByZ * Vector3.forward, true);
        for (int i = 0; i < MapController.CarbyCarCount - 1; i++)
        {
            PushEndRow();
        } 
    }

    private void PushEndRow()
    {
        Vector3 lastPosition = lastRow.transform.position;
        CraeteCarRow(lastPosition + MapController.DistanceByZ * Vector3.forward, false);
    }

    CarRowComponent CraeteCarRow(Vector3 newPosition, bool isFirst)
    {
        CarRowComponent newOne = EngageOne(newPosition);
        if (!isFirst)
            lastRow.NextRow = newOne;
        InitByRandomCars(newOne);
        lastRow = newOne;
        return newOne;
    }
	
	private void InitByRandomCars(CarRowComponent row)
    {
        int randomFreeCell = random.Next(MapController.RowCount);
		if (row.Cars == null)
			row.Cars = new CarRowComponent.Typle<GameObject, BasePool<Transform>>[MapController.RowCount];

		var cars = row.Cars;
        for (int i = 0; i < cars.Length; i++)
        {
			if (i == randomFreeCell)
			{
				continue;
			}

			int randomPoolNumber = random.Next(pools.Length);
			BasePool<Transform> pool = pools[randomPoolNumber];

			float newY = pools[randomPoolNumber].InitialComponent.transform.position.y;
			Transform carTransform = pools[randomPoolNumber].EngageOne(new Vector3());
			carTransform.SetParent(row.transform);
			Vector3 offset = new Vector3(MapController.LeftRowX + i * MapController.RowWidth, newY, 0);
			carTransform.localPosition = offset;

			row.Cars[i] = new CarRowComponent.Typle<GameObject, BasePool<Transform>>(carTransform.gameObject, pool);
		}
    }

	internal void NotifyAboutDestroy(CarRowComponent.Typle<GameObject, BasePool<Transform>>[] cars)
	{
		for (int i = 0; i < cars.Length; i++)
		{
			var pair = cars[i];
			cars[i] = new CarRowComponent.Typle<GameObject, BasePool<Transform>>();

			if (pair.Field2 == null)
				continue;
			pair.Field2.Free(pair.Field1.transform);
		}
		PushEndRow();
	}
}