using UnityEngine;

public class Striker : MonoBehaviour
{

	public RayFactory RayFactory;

	private const int DroneLayer = 1 << 12;
	public Camera Camera;
    public AudioSource shot;

    // Update is called once per frame
    void Update () {
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
		{
			Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

            shot.Play();
            float len = 1000;
			ShootingTrace rayModel = RayFactory.Engage();
			Vector3 start = transform.position + new Vector3(0, 1, 0);
			Vector3 stop = ray.GetPoint(len);
			start -= 0.1f * (stop - start);
			rayModel.gameObject.SetActive(true);
			rayModel.transform.position = (start + stop) / 2;
			rayModel.transform.up = stop - start;
			rayModel.transform.localScale = new Vector3(0.05f, len, 0.05f);


			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 1000, DroneLayer))
			{
				var droneScript = raycastHit.transform.GetComponent<DroneScript>();
				droneScript.Die();
			}
		}
	}
}
