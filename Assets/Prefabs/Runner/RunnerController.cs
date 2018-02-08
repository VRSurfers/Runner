using UnityEngine;

public class RunnerController : MonoBehaviour {

	class InputController
	{
		public static void Process(Transform transform)
		{
			if (Input.GetKey(KeyCode.A))
			{
				transform.position += -0.05f * transform.right;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				transform.position += 0.05f * transform.right;
			}
		}
	}

	void Update ()
	{
		InputController.Process(transform);
		transform.position += transform.forward * 0.1f;
	}
}
