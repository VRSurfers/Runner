using UnityEngine;

class InputController
{
	public static void GetmovementInfo(out float? horizontal, out float? vertical)
	{
#if UNITY_EDITOR
		PCInput(out horizontal, out vertical);
#elif UNITY_IOS || UNITY_ANDROID
		MobileInput(out horizontal, out vertical);
#endif
	}

	private static void PCInput(out float? horizontal, out float? vertical)
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
		if (Input.GetKeyDown(KeyCode.S))
		{
			vertical = -1f;
		}
		else if (Input.GetKeyDown(KeyCode.W))
		{
			vertical = +1f;
		}
	}

	private static Vector2 fingerDown;
	private static Vector2 fingerUp;
	public float SWIPE_THRESHOLD = 20f;

	private static void MobileInput(out float? horizontal, out float? vertical)
	{
		foreach (Touch touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Began)
			{
				fingerDown = touch.position;
			}
			if (touch.phase == TouchPhase.Ended)
			{
				fingerUp = touch.position;

				Vector2 delta = fingerUp - fingerDown;
				if (delta.magnitude > 20)
				{
					if (delta.x > 15)
					{
						horizontal = Mathf.Sign(delta.x);
						vertical = null;
						return;
					}
					else if (delta.y > 15)
					{
						horizontal = null;
						vertical = Mathf.Sign(delta.y);
						return;
					}
				}
			}
		}
		horizontal = null;
		vertical = null;
	}
}
