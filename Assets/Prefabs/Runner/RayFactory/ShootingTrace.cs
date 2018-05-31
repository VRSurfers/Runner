using UnityEngine;

public class ShootingTrace : MonoBehaviour
{
	public WorldMotionController WorldMotionController;
	public float Duration = 3;
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
	
	void Update ()
	{
		float liveTime = Time.time - awakeTime;
		if (liveTime > Duration)
		{
			WorldMotionController.Release(transform);
		}
		material.color = new Color(color.r, color.g, color.b, 1 - liveTime / Duration);
	}
}
