using UnityEngine;

public class Explosion : MonoBehaviour {

	public ParticleSystem ParticleSystem;
	[HideInInspector]
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
