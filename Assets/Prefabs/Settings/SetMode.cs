using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMode : MonoBehaviour {

    public Camera MainCamera;
    public GameObject VRCamera;
    public GameObject[] VRTools;
    public GameObject EventSystem;

	void Start () {
        if (SystemInfo.supportsGyroscope)
        {
            MainCamera.gameObject.SetActive(false);
            if (EventSystem != null)
                EventSystem.SetActive(false);
            VRCamera.SetActive(true);
            foreach (var element in VRTools)
            {
                element.SetActive(true);
            }
        }
    }
	
}
