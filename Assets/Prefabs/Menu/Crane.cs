using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Crane : MonoBehaviour {

    public Renderer craneRenderer;
    public Material darkerMaterial;
    public string sceneName;
    private Material mainMaterial;

    public Image black;
    public Animator animator;

    void Start () {
        mainMaterial = craneRenderer.material;
	}

    public void OnPointerClick()
    {
        StartCoroutine(Fade());
    }

    public void OnPointerEnter()
    {
        craneRenderer.material = darkerMaterial;
    }

    public void OnPointerExit()
    {
        craneRenderer.material = mainMaterial;
    }

    private IEnumerator Fade()
    {
        animator.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene(sceneName);
    }
}
