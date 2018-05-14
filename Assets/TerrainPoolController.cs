using UnityEngine;

public class TerrainPoolController : MonoBehaviour
{
	public WorldMotionController WorldMotionController;
	public float OffsetFromOther;
	public TerrainPiece Piece1;
	public TerrainPiece Piece2;

	private void Start()
    {
		WorldMotionController.Add(Piece1.transform);
		WorldMotionController.Add(Piece2.transform);
	}

	internal void OnCollision(TerrainPiece terrainPlate)
	{
		terrainPlate.transform.position = new Vector3(0, 0, (terrainPlate == Piece1 ? Piece2 : Piece1).transform.position.z + OffsetFromOther);
	}
}

