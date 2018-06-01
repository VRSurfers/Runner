using UnityEngine;

public class Striker : MonoBehaviour
{
	public WorldMotionController WorldMotionController;
	public DronManager DronManager;
	public ShootingTrace InitialRay;
    public float ShootingDelay = 1f;
	private BaseObjectPool pool;
	public Camera Camera;
    public AudioSource shot;

    private float lastShoot;

	public void Start()
	{
		InitialRay.WorldMotionController = WorldMotionController;
		pool = new BaseObjectPool(InitialRay.transform, DronManager.transform);
	}

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
                rayStopPoint = raycastHit.transform.position;
				if (raycastHit.transform.gameObject.layer == 17)
					DronManager.DestroyDrone(raycastHit.transform);
			}
            else
                rayStopPoint = ray.GetPoint(shootingLength);

            Transform rayModel = pool.Engage(new Vector3());
            Vector3 start = transform.position + new Vector3(0, 0, 0.1f);
            rayModel.gameObject.SetActive(true);
            rayModel.transform.position = (start + rayStopPoint) / 2;
            Vector3 rayTovector = rayStopPoint - start;
            rayModel.transform.up = rayTovector;
            const float rayThickness = 0.05f;
            rayModel.transform.localScale
                = new Vector3(rayThickness, rayTovector.magnitude, rayThickness);
			WorldMotionController.Add(rayModel, pool);
        }
	}
}
