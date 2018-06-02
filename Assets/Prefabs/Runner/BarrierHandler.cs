using System;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHandler : MonoBehaviour
{
    public RunerMotionController Runner;
	public TrackObjectsManager TrackObjectsManager;

    private void OnTriggerEnter(Collider other)
    {
	    CooliderBehaviour.OnEnter(other, this);
    }

	private void OnTriggerExit(Collider other)
	{
		Runner.StopCollision();
	}
}

static class KnowLayers
{
	public const int CarLayer = 19;
	public const int MedKitsLayer = 16;
	public const int AmoKitsLayer = 15;
}

static class CooliderBehaviour
{
	private static readonly Dictionary<int, Action<BarrierHandler, Collider>> Dictionary = new Dictionary<int, Action<BarrierHandler, Collider>>()
	{
		{ KnowLayers.CarLayer, CollisionWithCar},
		{ KnowLayers.MedKitsLayer, Healing},
		{ KnowLayers.AmoKitsLayer, Amo}
	};

	public static void OnEnter(Collider collider, BarrierHandler barrierHandler)
	{
		Action<BarrierHandler, Collider> handler;
		if (Dictionary.TryGetValue(collider.gameObject.layer, out handler))
		{
			handler(barrierHandler, collider);
		}
	}

	public const float Size = 0.95f;

	private static void CollisionWithCar(BarrierHandler barrierHandler, Collider collider)
	{
		RunerMotionController runner = barrierHandler.Runner;
		Vector3 runnerPosition = runner.transform.position;
		Vector3 carPosition = collider.transform.position;

		if (Math.Abs(runnerPosition.x - carPosition.x) < Size)
		{
			runner.FrontCollision(barrierHandler.TrackObjectsManager.GetNextFreeX(collider.transform.parent.transform));
		}
		else
		{
			runner.SideCollision();
		}
	}





	public const float HPinKit = 20f;

	private static void Healing(BarrierHandler barrierHandler, Collider collider)
	{
		barrierHandler.Runner.HealthComponent.Change(HPinKit);
		barrierHandler.TrackObjectsManager.ReleaseKit(collider.transform.parent.transform);
	}

	public const int AmoInKit = 5;

	private static void Amo(BarrierHandler barrierHandler, Collider collider)
	{
		barrierHandler.Runner.AmoComponent.Change(AmoInKit);
		barrierHandler.TrackObjectsManager.ReleaseKit(collider.transform.parent.transform);
	}
}
