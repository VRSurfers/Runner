using UnityEngine;

class InputController
{
	public static void GetmovementInfo(out float? horizontal, out float? vertical)
	{
		horizontal = null;
		if (Input.GetKey(KeyCode.A) || LeftVR())
		{
			horizontal = -1f;
		}
		else if (Input.GetKey(KeyCode.D) || RightVR())
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

    private static bool LeftVR()
    {
        return Camera.main.transform.rotation.z > 0.25f ? true : false;
    }

    private static bool RightVR()
    {
        return Camera.main.transform.rotation.z < -0.25f ? true : false;
    }
}
