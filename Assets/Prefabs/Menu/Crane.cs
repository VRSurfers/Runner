using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Crane : MonoBehaviour {

    public Renderer craneRenderer;
    public Material darkerMaterial;
    public TextMesh Instruction;
    public string sceneName;
    private Material mainMaterial;
    private bool GameIsLoading = false;

    public Image black;
    public Animator animator;

    void Start () {
        mainMaterial = craneRenderer.material;
	}

    public void OnPointerClick()
    {
        if (SystemInfo.supportsGyroscope && !GameIsLoading)
        {
            GameIsLoading = true;
            Instruction.text = "Please wait... Loading level";
            StartCoroutine(AsyncLoad());
        }
        else if(!GameIsLoading)
        {
            GameIsLoading = true;
            StartCoroutine(Fade());
        }          
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

    private IEnumerator AsyncLoad()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
}
