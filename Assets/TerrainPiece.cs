using UnityEngine;

public class TerrainPiece : MonoBehaviour
{
	public TerrainPoolController TerrainPoolController;

	private void OnTriggerEnter(Collider other)
	{
		TerrainPoolController.OnCollision(this);
	}
}

