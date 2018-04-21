using System;
using UnityEngine;

public class DroneScript : MonoBehaviour {

	public AudioSource ExplosionSound;
	public RunnerController Target;
	public DronManager Owner;

	public float FullLifeTime = 10;
	public float HangingHeight = 2;
	public float VerticalSpeed = 2;
	public float DistanceOfDaamage = 20f;
	public float ShootingDeltaTime = 1f;
	public float DamegePerShot = 1f;

	private float beginLifeTime;
	private float lastShootTime;

	void Start () {
		transform.rotation = Quaternion.Euler(-90, 0, 0);
	}

	private void OnEnable()
	{
		beginLifeTime = Time.time;
	}

	internal void Die()
	{
		Owner.FreeDrone(this, true);
        ExplosionSound.Play();
	}

	// Update is called once per frame
	void Update () {
		ProcessIsAlive();
		TryDamageTarget();
		CorrectHeight();
		CorrectRotation();
	}

	private void TryDamageTarget()
	{
		float currentTime = Time.time;
		if (currentTime - lastShootTime > ShootingDeltaTime)
		{
			if ((transform.position - Target.transform.position).magnitude < DistanceOfDaamage)
			{
				lastShootTime = currentTime;
				Target.HP -= DamegePerShot;
			}
		}
	}

	private void ProcessIsAlive()
	{
		if (Time.time - beginLifeTime > FullLifeTime)
		{
			Owner.FreeDrone(this, false);
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
