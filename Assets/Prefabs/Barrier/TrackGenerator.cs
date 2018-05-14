using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrackGenerator : MonoBehaviour {

    public GameObject Barriers;
    public GameObject player;
    public Animator animator;
    public Image black;
    public string sceneName;

    private Queue<GameObject> cars = new Queue<GameObject>();
    private float LastPosition = 13.0f;
    private float FirstPosition = 13.0f;
    private int PreviouseRoad = 1;
    private float deltaDistance = 7.0f;
    private float longDeltaDistance = 11.0f;
    private int lengthOfVisibleTrack = 12;
    private float winDistance = 580.0f;
    private float endOfGeneration = 500.0f;
    private bool win = false;
    private bool isGyroscope;

    enum CoordinatesOfTrack
    {
        left = 1,
        center = 4,
        right = 7
    }

    enum Tracks
    {
        left,
        center,
        right
    }

    void Start()
    {
        isGyroscope = SystemInfo.supportsGyroscope;
        cars = GetObstacles();
        for (int i = 0; i < lengthOfVisibleTrack; i++)
        {
            GeneratePath();
        }      
	}

    void Update()
    {
        if (NewRespawn() && player.transform.position.z < endOfGeneration)
        {
            for (int i = 0; i < lengthOfVisibleTrack / 2; i++)
            {
                GeneratePath();
            }
        }
        CheckTheWin();
    }

    private void GeneratePath()
    {
        int path = Random.Range(0, 3);
        if (Mathf.Abs(path-PreviouseRoad) == 2)
        {
            LastPosition += longDeltaDistance;
        }
        else
        {
            LastPosition += deltaDistance;
        }
        for (int j = 0; j < 3; j++)
        {
            if (j != path)
            {
                int positionX = GetXPosition(j);
                GameObject currentObstacle = cars.Dequeue();
                currentObstacle.SetActive(true);
                currentObstacle.transform.position = new Vector3(positionX, currentObstacle.transform.position.y, LastPosition);
                cars.Enqueue(currentObstacle);
            }
        }
        PreviouseRoad = path;
    }

    private int GetXPosition(int trackNum)
    {
        if (trackNum == (int)Tracks.left)
        {
            return (int)CoordinatesOfTrack.left;
        }
        else if (trackNum == (int)Tracks.center)
        {
            return (int)CoordinatesOfTrack.center;
        }
        else
        {
            return (int)CoordinatesOfTrack.right;
        }
    }

    private Queue<GameObject> GetObstacles()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Barriers");
        var queueOfObstacles = new Queue<GameObject>();
        foreach (var obstacle in obstacles)
        {
            queueOfObstacles.Enqueue(obstacle);
        }
        return queueOfObstacles;
    }

    private void CheckTheWin()
    {
        if (player.transform.position.z > winDistance && isGyroscope && !win)
        {
            win = true;
            StartCoroutine(AsyncLoad());
        }
        else if (player.transform.position.z > winDistance && !win)
        {
            win = true;
            StartCoroutine(Fade());
        }    
    }

    private bool NewRespawn()
    {
        if (player.transform.position.z > (LastPosition + FirstPosition) / 2 + longDeltaDistance)
        {
            FirstPosition = player.transform.position.z;
            return true;
        }
        else
        {
            return false;
        }
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
