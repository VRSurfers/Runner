using UnityEngine;

public class TerrainPoolController : MonoBehaviour, IReleaser
{
	public WorldMotionController WorldMotionController;
	public float OffsetFromOther;
	public Transform Piece1;
	public Transform Piece2;

	private void Start()
    {
		WorldMotionController.Add(Piece1, this);
		WorldMotionController.Add(Piece2, this);
	}

	public void Release(Transform objTransform)
	{
		objTransform.position = new Vector3(0, 0, (objTransform == Piece1 ? Piece2 : Piece1).transform.position.z + OffsetFromOther);
		WorldMotionController.Add(objTransform, this);
	}
}

