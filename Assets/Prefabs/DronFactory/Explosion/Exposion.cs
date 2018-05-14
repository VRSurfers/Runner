using UnityEngine;

public class Exposion : PooledObject {

	public ParticleSystem ParticleSystem;
	public ExplosionFactory ExplosionFactory;

	private float startLife;
    private float duration;

    private void OnEnable()
	{
		startLife = Time.time;
        duration = ParticleSystem.main.duration;
    }

	// Update is called once per frame
	void FixedUpdate () {
		if (Time.time - startLife > ParticleSystem.main.startLifetimeMultiplier + duration)
		{
            ReturnToPool();
		}
	}
}
