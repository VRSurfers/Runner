using UnityEngine;

public class Striker : MonoBehaviour {

	private const int DroneLayer = 1 << 12;
	public Camera Camera;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
		{
			Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 1000, DroneLayer))
			{
				var droneScript = raycastHit.transform.GetComponent<DroneScript>();
				droneScript.Die();
			}
		}
	}
}
