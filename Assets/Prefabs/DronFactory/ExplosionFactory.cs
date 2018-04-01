using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFactory : MonoBehaviour {

	public ParticleSystem InitialExplosion;
	private Stack<ParticleSystem> explosiosPool = new Stack<ParticleSystem>();

	internal void Boom(Transform boomtransform)
	{
		ParticleSystem newExpolsion;
		if (explosiosPool.Count == 0)
		{
			newExpolsion = Instantiate(InitialExplosion, transform);
		}
		else
		{
			newExpolsion = explosiosPool.Pop();
		}
		newExpolsion.transform.position = boomtransform.position;
		newExpolsion.gameObject.SetActive(true);
	}

	internal void Return(ParticleSystem particleSystem)
	{
		particleSystem.gameObject.SetActive(false);
		explosiosPool.Push(particleSystem);
	}
}
