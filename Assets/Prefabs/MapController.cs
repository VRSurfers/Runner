using UnityEngine;

public class MapController : MonoBehaviour {

    public int CarSeriesCount = 5;
    public int RowCount = 3;
    public float DistanceByZ = 10;
    public float LeftRowX = -2f;
    public float RowWidth = 3.3f;

	public float GetRghtX()
	{
		return LeftRowX + (RowCount - 1) * RowWidth;
	}
}
