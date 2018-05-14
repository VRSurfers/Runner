using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTrace : MonoBehaviour
{
	public float Duration = 3;
	public RayFactory RayFactory;
	private float awakeTime;
	private Material material;
	private Color color;
	
	void OnEnable()
	{
		material = GetComponent<Renderer>().material;
		awakeTime = Time.time;
		color = material.color;
		color.a = 1f;
		material.color = color;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float liveTime = Time.time - awakeTime;
		if (liveTime > Duration)
		{
			RayFactory.Free(this);
		}
		material.color = new Color(color.r, color.g, color.b, 1 - liveTime / Duration);
	}
}
