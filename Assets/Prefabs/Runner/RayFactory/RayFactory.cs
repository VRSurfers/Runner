using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayFactory : MonoBehaviour {

	private readonly Stack<ShootingTrace> stack = new Stack<ShootingTrace>();
	public ShootingTrace Ray;

	public ShootingTrace Engage()
	{
		ShootingTrace ray = stack.Count == 0 ? Instantiate(Ray) : stack.Pop();
		return ray;
	}

	public void Free(ShootingTrace ray)
	{
		stack.Push(ray);
		ray.gameObject.SetActive(false);
	}
}
