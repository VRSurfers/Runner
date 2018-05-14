using UnityEngine;

public class Striker : MonoBehaviour
{
    public float ShootingDelay = 1f;
	public RayFactory RayFactory;
    public Camera Camera;
    public AudioSource shot;

    private float lastShoot;

    void Update () {       
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
		{
            if (lastShoot + ShootingDelay > Time.time)
                return;

            lastShoot = Time.time;
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

            //shot.Play();
			RaycastHit raycastHit;
            Vector3 rayStopPoint;
            const float shootingLength = 1000f;
            if (Physics.Raycast(ray, out raycastHit, shootingLength))
            {
                var droneScript = raycastHit.transform.GetComponent<DroneScript>();
                if (droneScript != null)
                    droneScript.Die();
                rayStopPoint = raycastHit.transform.position;
            }
            else
                rayStopPoint = ray.GetPoint(shootingLength);

            ShootingTrace rayModel = RayFactory.Engage();
            Vector3 start = transform.position + new Vector3(0, 0, 0.1f);
            rayModel.gameObject.SetActive(true);
            rayModel.transform.position = (start + rayStopPoint) / 2;
            Vector3 rayTovector = rayStopPoint - start;
            rayModel.transform.up = rayTovector;
            const float rayThickness = 0.05f;
            rayModel.transform.localScale
                = new Vector3(rayThickness, rayTovector.magnitude, rayThickness);
        }
	}
}
