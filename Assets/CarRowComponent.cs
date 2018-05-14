using System;
using UnityEngine;

public class CarRowComponent : PooledObject
{
	public CarManager CarManager;

	[NonSerialized]
	public Typle<GameObject, BasePool<Transform>>[] Cars;

    public CarRowComponent NextRow;

    public override void ReturnToPool()
    {
        CarManager.NotifyAboutDestroy(Cars);
        base.ReturnToPool();
    }

	public struct Typle<T1, T2>
	{
		public readonly T1 Field1;
		public readonly T2 Field2;

		public Typle(T1 field1, T2 field2)
		{
			Field1 = field1;
			Field2 = field2;
		}
	}

    internal float GetFreeX()
    {
        for (int i = 0; i < Cars.Length; i++)
        {
            if (Cars[i].Field1 == null)
                return CarManager.MapController.LeftRowX + i * CarManager.MapController.RowWidth;
        }
        throw new InvalidOperationException();
    }
}
