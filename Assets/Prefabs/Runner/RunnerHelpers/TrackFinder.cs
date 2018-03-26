using UnityEngine;

internal class TrackFinder
{
	private const float MaxRaycastLen = 100f;
	private const int TrackLayer = 1 << 8;

	internal static Vector3? ExistTrackInDirection(Transform transform, float rightSide)
    {
		Vector3 raycastDirection = rightSide * Vector3.right;
		RaycastHit raycastHit;
		bool wasHit = Physics.Raycast(
				transform.position + raycastDirection,
				raycastDirection,
				out raycastHit,
				MaxRaycastLen, TrackLayer);
		if (wasHit)
			return DirectionHelper.GetDirectionFromTrack(raycastHit.transform);
		else
			return null;
    }
}