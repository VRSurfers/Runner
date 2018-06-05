using UnityEngine;

public class TerrainPoolController : MonoBehaviour, IReleaser
{
	public WorldMotionController WorldMotionController;
	public float OffsetFromOther;
	public Transform[] Terains;
	public Transform LastTerain;

	private void Start()
    {
		for (int i = 0; i < Terains.Length; i++)
		{
			WorldMotionController.Add(Terains[i], this);
		}
	}

	public void Release(Transform objTransform)
	{
		objTransform.position = new Vector3(0, 0, LastTerain.position.z + OffsetFromOther);
		WorldMotionController.Add(objTransform, this);
		LastTerain = objTransform;
	}
}

