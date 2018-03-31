using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackGenerator : MonoBehaviour {

    public List<GameObject> cars = new List<GameObject>();
    public List<GameObject> barriers = new List<GameObject>();
    public GameObject player;
    float LastPositin = 13;
    int PreviouseRoad = 1;
    float deltaDistance = 7.0f;
    float longDeltaDistance = 11.0f;
    float winDistance = 510.0f;

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

    void Start () {
        GeneratePath();
	}

    private void Update()
    {
        if (player.transform.position.z > winDistance)
        {
            SceneManager.LoadScene("Menu");
        }
    }

    void GeneratePath()
    {
        for (int i = 0; i < 54; i++)
        {
            int path = Random.Range(0, 3);
            if (path != (int)Tracks.center && path != PreviouseRoad)
            {
                LastPositin += longDeltaDistance;
            }
            else
            {
                LastPositin += deltaDistance;
            }
            for (int j = 0; j < 3; j++)
            {
                if (j != path)
                {
                    int positionX = GetXPosition(j);
                    int index = Random.Range(0, cars.Count);
                    Instantiate(cars[index], new Vector3(positionX, cars[index].transform.position.y, LastPositin), cars[index].transform.rotation);
                }
            }
            PreviouseRoad = path;
        }
    }

    int GetXPosition(int trackNum)
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
}
