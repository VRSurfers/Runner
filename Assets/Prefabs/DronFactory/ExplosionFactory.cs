using UnityEngine;

public class ExplosionFactory : PoolWithMoving<PooledObject>
{
	internal void Boom(Transform boomtransform)
	{
        EngageOne(boomtransform.position);
	}
}
