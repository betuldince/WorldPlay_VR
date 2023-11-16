/// \file
/// \brief ObstacleSpawnerController is used to control the various aspects of the obstacle spawner. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is used to control the various aspects of the obstacle spawner. 
/// </summary>
/// It is configured as [ExecuteInEditMode] so that the spawn locations can be visibly modified in the edit mode. 
[ExecuteInEditMode]
public class ObstacleSpawnerController : MonoBehaviour {

    [Range(0.0f, 2.0f)]
    public float playAreaX, playAreaZ;
    /// <summary>
    /// The obstacle to be spawned.
    /// </summary>
    public GameObject obstacle;
    /// <summary>
    /// It sets the obstacle placeholder object which is visible in the edit mode. 
    /// </summary>
    public GameObject obstacleIndicator;
    /// <summary>
    /// A list of all the obstacles in the scene.
    /// </summary>
    List<GameObject> obstacles;
    /// <summary>
    /// The maximum number of times the spawner should restart.
    /// </summary>
    public int spawnerTrialCount = 500;
    /// <summary>
    /// The number of times the random locations fail to restart the spawner. 
    /// </summary>
    public int innerTrialCount = 500;
    /// <summary>
    /// Enabling this helps in optimizing the trialCount and reduce the time it takes for the spawner to start.
    /// </summary>
    public bool enableSpawnerDebugLog = true;

    /// <summary>
    /// The function to be called to spawn obstacles. 
    /// </summary>
    public List<GameObject> spawnObstacles(int numberOfObstacles)
    {
        clearObstacles();
        ///Try restarting the obstacle spawner 'spawnerTrialCount' number of times.
        for (int t = 0; t < spawnerTrialCount; t++)
        {
            if (obstacles.Count < numberOfObstacles)
            {
                if (enableSpawnerDebugLog)
                    Debug.Log("Restarting spawner, trial number: " + t);
                clearObstacles();
                for (int obstCount = 0; obstCount < numberOfObstacles; obstCount++)
                {
                    Vector2 newPos = new Vector2(0, 0);
                    bool invalidPos = false;
                    ///Limit the maximum number of times the random position is generated.
                    for (int trialNo = 0; trialNo < innerTrialCount; trialNo++)
                    {
                        newPos = new Vector2(Random.Range(transform.position.x - playAreaX, transform.position.x + playAreaX), Random.Range(transform.position.z - playAreaZ, transform.position.z + playAreaZ));
                        Vector3 obstaclePos = transform.position;
                        obstaclePos.x = newPos.x;
                        obstaclePos.z = newPos.y;
                        Quaternion randRot = Quaternion.identity;
                        randRot *= Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
                        GameObject tempObstacle = Instantiate(obstacle, obstaclePos, randRot);
                        invalidPos = false;
                        ///Check if the newPos is too close to any of the old obstacles. 
                        foreach (GameObject obs in obstacles)
                        {
                            ///If any object intersects with the new location, break the loop. 
                            if (tempObstacle.GetComponent<BoxCollider>().bounds.Intersects(obs.GetComponent<BoxCollider>().bounds))
                            {
                                invalidPos = true;
                                break;
                            }
                        }
                        ///If we have found a new position with enough clearance.
                        if (!invalidPos)
                        {
                            obstacles.Add(tempObstacle);
                            tempObstacle.transform.parent = transform;
                            if (enableSpawnerDebugLog)
                                Debug.Log("Number of obstacles spawned: " + obstacles.Count + " numberOfObstacles: " + numberOfObstacles + " Inner trial number: " + trialNo);
                            break;
                        }
                        else
                        {
                            DestroyImmediate(tempObstacle);
                        }
                    }
                    ///If the trialCount number of loops to find a free spot for the object has failed, restart the object spawner. 
                    if (invalidPos)
                        break;
                }
            }
            else
                break;
        }
        return obstacles;
    }

    /// <summary>
    /// Deletes all the obstracles and obstacle indicator planes. 
    /// </summary>
    void clearObstacles()
    {
        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        obstacles.Clear();
    }

	// Use this for initialization
	void Start () {
        ///Delete the obstacle indicator planes from the editor.
        obstacles = new List<GameObject>();
        clearObstacles();
    }
	

	// Update is called once per frame
	void Update () {
        ///Show the obstacle spawn area in the editor. 
        if (Application.isEditor && !Application.isPlaying)
        {
            ///Delete all existing indicator planes before regenerating them in the editor.
            while (transform.childCount != 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            
            for(int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                {
                    Vector3 planePos = transform.position;
                    Quaternion planeRot = Quaternion.identity;
                    planePos.x += (1 - (2 * i)) * (playAreaX);
                    planePos.z += (1 - (2 * j)) * (playAreaZ);
                    for(int k = 0; k < 4; k++)
                    {
                        planeRot *= Quaternion.Euler(Vector3.up * k * 45);
                        GameObject tempSpawnedPlane = Instantiate(obstacleIndicator, planePos, planeRot);
                        tempSpawnedPlane.transform.parent = transform;
                    }
                    
                }                 
        }
    }
}

