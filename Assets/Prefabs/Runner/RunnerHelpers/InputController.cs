using UnityEngine;

class InputController
{
	public static void GetmovementInfo(out float? horizontal, out float? vertical)
	{
		horizontal = null;
		if (Input.GetKey(KeyCode.A))
		{
			horizontal = -1f;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			horizontal = +1f;
		}

		vertical = null;
		if (Input.GetKey(KeyCode.S))
		{
			vertical = -1f;
		}
		else if (Input.GetKey(KeyCode.W))
		{
			vertical = +1f;
		}
	}
}
