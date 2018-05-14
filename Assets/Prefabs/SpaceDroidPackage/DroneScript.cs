using System;
using UnityEngine;

public class DroneScript : PooledObject {

	public AudioSource ExplosionSound;
	public RunnerController Target;
    public ExplosionFactory ExplosionFactory;

	public float FullLifeTime = 10;
	public float HangingHeight = 2;
	public float VerticalSpeed = 2;
	public float DistanceOfDaamage = 20f;
	public float ShootingDeltaTime = 1f;
	public float DamegePerShot = 1f;

    private Health health;
    private float lastShootTime;

    void Start () {
		transform.rotation = Quaternion.Euler(-90, 0, 0);
        health = Target.GetComponent<Health>();
	}

	internal void Die()
	{
        ExplosionFactory.Boom(transform);
        ReturnToPool();
        //ExplosionSound.Play();
	}

	// Update is called once per frame
	void Update () {
		CorrectHeight();
		CorrectRotation();
        TryDamageTarget();
    }

	private void TryDamageTarget()
	{
		float currentTime = Time.time;
		if (currentTime - lastShootTime > ShootingDeltaTime)
		{
			if ((transform.position - Target.transform.position).magnitude < DistanceOfDaamage)
			{
				lastShootTime = currentTime;
                health.HP -= DamegePerShot;
			}
		}
	}

	void CorrectHeight()
	{
		Vector3 position = transform.position;
		if (HangingHeight != position.y)
		{
			float delta = HangingHeight - transform.position.y;
			position.y += Math.Sign(delta) * Math.Min(VerticalSpeed * Time.deltaTime, Math.Abs(delta));
			transform.position = position;
		}
	}

	void CorrectRotation()
	{
		transform.rotation = Quaternion.Euler(-90, 0, GetZRotOnTarget());
	}

	float GetZRotOnTarget()
	{
		Vector3 targetDirection = Target.transform.position - transform.position;
		targetDirection.y = 0;

		float a1 = Vector3.Angle(Vector3.forward, targetDirection);

		float a2 = Vector3.Angle(Vector3.right, targetDirection);
		a2 = a2 - 90 <= 0 ? +1 : -1;
		return a2 * a1;
	}
}
