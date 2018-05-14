using UnityEngine;

class InputController
{
	public static void GetmovementInfo(out float? horizontal, out float? vertical)
	{
		horizontal = null;
		if (Input.GetKeyDown(KeyCode.A))
		{
			horizontal = -1f;
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			horizontal = +1f;
		}

		vertical = null;
		if (Input.GetKeyDown(KeyCode.S))
		{
			vertical = -1f;
		}
		else if (Input.GetKeyDown(KeyCode.W))
		{
			vertical = +1f;
		}
	}
}
