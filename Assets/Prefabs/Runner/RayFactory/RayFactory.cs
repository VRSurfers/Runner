using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayFactory : MonoBehaviour {

    public WorldMotionController WorldMotionController;
	private readonly Stack<ShootingTrace> stack = new Stack<ShootingTrace>();
	public ShootingTrace Ray;

	public ShootingTrace Engage()
	{
		ShootingTrace ray = stack.Count == 0 ? Instantiate(Ray, transform) : stack.Pop();
        WorldMotionController.Add(ray.transform);

        return ray;
	}

	public void Free(ShootingTrace ray)
	{
		stack.Push(ray);
        WorldMotionController.Remove(ray.transform);
        ray.gameObject.SetActive(false);
	}
}
