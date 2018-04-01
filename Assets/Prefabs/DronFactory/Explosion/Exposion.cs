using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exposion : MonoBehaviour {

	public ParticleSystem ParticleSystem;
	public ExplosionFactory ExplosionFactory;

	private float startLife;

	private void OnEnable()
	{
		startLife = Time.time;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Time.time - startLife > ParticleSystem.main.startLifetimeMultiplier + ParticleSystem.main.duration)
		{
			ExplosionFactory.Return(ParticleSystem);
		}
	}
}
