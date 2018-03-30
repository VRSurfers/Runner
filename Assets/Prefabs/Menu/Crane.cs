using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Crane : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    public Renderer craneRenderer;
    public Material darkerMaterial;
    public string sceneName;
    private Material mainMaterial;

    void Start () {
        mainMaterial = craneRenderer.material;
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        craneRenderer.material = darkerMaterial;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        craneRenderer.material = mainMaterial;
    }
}
