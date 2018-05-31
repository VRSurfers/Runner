using UnityEngine;

public class Exposion : PooledObject {

	public ParticleSystem ParticleSystem;
	public WorldMotionController WorldMotionController;

	private float startLife;
    private float duration;

    private void OnEnable()
	{
		startLife = Time.time;
        duration = ParticleSystem.main.duration;
    }

	void Update () {
		if (Time.time - startLife > ParticleSystem.main.startLifetimeMultiplier + duration)
		{
			WorldMotionController.Release(transform);
		}
	}
}
