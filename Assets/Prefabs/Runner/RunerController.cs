using System;
using System.Text;
using UnityEngine;

public struct InputArgs
{
	public readonly float? Horizontal;
	public readonly Vector3? Fired;
	public readonly bool Jumped;

	public InputArgs(float? horizontal, Vector3? fired, bool jumped)
	{
		Horizontal = horizontal;
		Fired = fired;
		Jumped = jumped;
	}
}

public class RunerController : MonoBehaviour {

	public RunerMotionController RunerMotionController;
	public Striker Striker;

	private StringBuilder message = new StringBuilder();

	public void OnGUI()
	{
		//GUILayout.Label(message.ToString());
	}

	void Update ()
	{
		InputArgs inputArgs = new InputArgs();
		InputController.GetInputArgs(out inputArgs, message);
		RunerMotionController.ProcessMotionStep(ref inputArgs);
		if (inputArgs.Fired.HasValue)
			Striker.Shot(inputArgs.Fired.Value);
	}

	class InputController
	{
		internal static void GetInputArgs(out InputArgs inputArgs, StringBuilder message)
		{
#if UNITY_EDITOR
			PCInput(out inputArgs);
#elif UNITY_IOS || UNITY_ANDROID
			MobileInput(out inputArgs, message);
#endif
		}

		private static void PCInput(out InputArgs inputArgs)
		{
			float? horizontal = null;
			if (Input.GetKey(KeyCode.A))
			{
				horizontal = -1f;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				horizontal = +1f;
			}

			Vector3? firePosition = null;
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
			{
				firePosition = Input.mousePosition;
			}


			inputArgs = new InputArgs(
				horizontal,
				firePosition,
				Input.GetKeyDown(KeyCode.W));
		}

		// TODO: move to mobile processing
		private static Vector2 fingerDown;
		private const float ScreenPercent = 0.2f;
		private const float FireDelta = 7f;

		private static void MobileInput(out InputArgs inputArgs, StringBuilder message)
		{
			//log("touches " + Input.touches.Length);
			foreach (Touch touch in Input.touches)
			{
				if (touch.phase == TouchPhase.Began)
				{
					message.Remove(0, message.Length);
					message.AppendLine("Down in " + touch.position);
					fingerDown = touch.position;
				}
				if (touch.phase == TouchPhase.Ended)
				{
					message.AppendLine("Up in " + touch.position);
					Vector2 fingerUp = touch.position;
					message.AppendLine("Up " + fingerUp + "   Down " + fingerDown);

					Vector2 delta = fingerUp - fingerDown;
					if (delta.magnitude < FireDelta)
					{
						inputArgs = new InputArgs(null, (fingerUp + fingerDown) / 2, false);
						return;
					}

					float xAbs = Mathf.Abs(delta.x);
					float yAbs = Mathf.Abs(delta.y);

					if (xAbs > yAbs)
					{
						if (xAbs > Screen.width * ScreenPercent)
						{
							inputArgs = new InputArgs(Mathf.Sign(delta.x), null, false);
							return;
						}
					}
					else
					{
						inputArgs = new InputArgs(null, null, delta.y > 0);
						return;
					}
				}
			}
			inputArgs = new InputArgs(null, null, false);
		}
	}

}
