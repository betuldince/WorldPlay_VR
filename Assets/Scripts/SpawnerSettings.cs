/// \file
/// \brief SpawnerSettings is used to control the various aspects of the object spawner. 
/// 
/// It is designed to be as customizable as possible. The customizability allows for a increased playability based on the player's individual preferences.
/// A short description of how it works:
///     Gameplay:
///         ->The letters are spawned in start
///         ->In update we check if any letter has "fallen" and respawn it in a new spot in the grid.
///         ->If the time interval for shuffling has passed, we add objects to the toFly list and determine its 
///           location it should fly to.
///         ->Objects are flown over the next few updates until it has reached its final location.
///         ->The floating effect is restored after it has flown.
///     Editor:
///         ->We use unity planes to mark the spots where the letters are to be spawned.
///         ->To see this temporarily enable the spawner in the editor.
///         ->It also indicates the height range of where the letters are to be spawned.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

/// <summary>
/// This class is used to control the various aspects of the object spawner. 
/// </summary>
/// It is configured as [ExecuteInEditMode] so that the spawn locations can be visibly seen and modified in the edit mode. 
[ExecuteInEditMode]
public class SpawnerSettings : MonoBehaviour
{
    /// <summary>
    /// It sets the spawner placeholder object which is visible in the edit mode. 
    /// </summary>
    public GameObject spawnerPlane;
    /// <summary>
    /// <B>DEPRECATED?</B> Originally intended to spawn each object more than once. 
    /// </summary>
    public bool spawnAtMultiplePlaces = false;
    /// <summary>
    /// Sets the number of rows for the object spawner.
    /// </summary>
    /// Changes to this can be visibly seen in the edit mode.
    public int noOfRows = 1;
    /// <summary>
    /// Sets the number of columns for the object spawner.
    /// </summary>
    /// Changes to this can be visibly seen in the edit mode.
    public int noOfColumns = 1;
    /// <summary>
    /// Sets the space between the rows and columns of the object spawner. 
    /// </summary>
    /// The actual value of the space between the rows and columns is calculated by multiplying with the \link spacingScaleFactor \endlink. 
    /// Changes to this can be visibly seen in the edit mode.
    public float spacing = 1;
    public float spacingx = 1;
    public float spacingz = 1;
    /// <summary>
    /// Sets the minimum height the spawned object can appear at.
    /// </summary>
    /// Note: Changes to this can be visibly seen in the edit mode.
    public float minHeight = 0;
    /// <summary>
    /// Sets the maximum height the spawned object can appear at.
    /// </summary>
    /// Note: Changes to this can be visibly seen in the edit mode.
    public float maxHeight = 0;
    /// <summary>
    /// Create the parameters for the height based region division.
    /// </summary>
    /// We would like to space out the solution letters such that they are equally spaced out vertically always.
    float heightDivision1 = 0, heightDivision2 = 0;


    /////User Customization Setup/////

    /// <summary>
    /// It defines the space left for the user.
    /// </summary>
    /// If the value is 0, no space is left; if the value is 1, 1 to 4 squares are left in the centre depending on the number of \link noOfRows rows \endlink and \link noOfColumns columns\endlink. Each increment adds a layer of squares.
    public int userArea = 1;
    /// <summary>
    /// If the spawned objects always face the user's eye always or not. 
    /// </summary>
    public bool alphabetsFaceUsersEye = true;
    /// <summary>
    /// Specify a 3d point (userEyePoint) for the spawned objects to face, instead of facing the user. Set the point using \link userEyePoint \endlink.
    /// </summary>
    public bool useCustomUserEyeLocation = false;
    /// <summary>
    /// Set the 3d point at which the spawned objects always face. 
    /// </summary>
    /// Obeys \link useCustomUserEyeLocation \endlink specification.
    public Vector3 userEyePoint;
    /// <summary>
    /// Limit the maximum tilt of spawned objects to a certain angle (specified by maximumPitchChange) so the spawned objects never orient themselves flat, or tilt excessively.
    /// </summary>
    public bool limitMaximumPitchChange = false;
    /// <summary>
    /// How much the tilt should be restricted to. Obeys \link limitMaximumPitchChange \endlink specification. 
    /// </summary>
    public float maximumPitchChange = 30.0f;
    

    /////Spawn Dynamics/////
    
    public float spawnIntensity = 100;
    /// <summary>
    /// The interval between the spawned objects disappearing and reappearing.
    /// </summary>
    public float spawnInterval = 3;
    /// <summary>
    /// Enable or disable gravity for all the objects in the scene. Keep disabled. 
    /// </summary>
    public bool gravity = false;
    /// <summary>
    /// Enable or disable a up-down oscialltion of spawned objects to imitate floating. 
    /// </summary>
    public bool floatingEffect = true;
    /// <summary>
    /// A temp variable used to save the current floating state of the spawned objects. 
    /// </summary>
    bool tempFloatingEffect = true;
    /// <summary>
    /// The height of the oscillation.
    /// </summary>
    public float height = 0.01f;
    /// <summary>
    /// The frequency of the oscillation.
    /// </summary>
    public float frequency = 1;
    /// <summary>
    /// Enable or diable a spinning effect for the spawned objects. Set the speed of the rotation using \link rotationSpeed \endlink.
    /// </summary>
    public bool spin = false;
    /// <summary>
    /// Sets the speed at which the alphabets should rotate. 
    /// </summary>
    /// Obeys \link spin \endlink specification.
    public float rotationSpeed = 15.0f;
    /// <summary>
    /// If the spawned objects can repeat in the playing field.
    /// </summary>
    public bool repetition = true;
    /// <summary>
    /// THe maximum number of times the spawned objects can repeat.
    /// </summary>
    /// Obeys \link repetition \endlink specification.
    public int repetitionCount = 0;
    /// <summary>
    /// Manually specify a solution string. Does not overwrite the string received from the network by default.
    /// </summary>
    public string solutionString;
    /// <summary>
    /// If all the alphabets other than soluton alphabets are to be spawned. 
    /// </summary>
    public bool spawnAllOtherAlphabets = true;
    /// <summary>
    /// If all spawnAllOtherAlphabets is not enabled, alphabets specified in this string will be spawned.  
    /// </summary>
    /// Obeys \link repetition \endlink specification.
    public string otherAlphabetsToSpawn = "ajhgklscbzx";
    /// <summary>
    /// A list of Game Objects which need to be spawned later. 
    /// </summary>
    public List<GameObject> gameObjects;
    /// <summary>
    /// A list of only the solution gameobjects. 
    /// </summary>
    public List<GameObject> SolutionGameObjects;
    /// <summary>
    /// Sets the level the spawner belongs to. 
    /// </summary>
    public int spawnerNumber = 0;
    /// <summary>
    /// A list of all the occcupied spots in the playing field grid's. 
    /// </summary>
    List<Vector2> existingObjs;
    /// <summary>
    /// <B>DEPRECATED</B> Used to indicate a manual solution string. 
    /// </summary>
    string solution;
    /// <summary>
    /// A local member indicating where the alphabets should face.
    /// </summary>
    Vector3 centerPoint;
    /// <summary>
    /// The unit of the spacing, the actual space between the grids is \link spacing \endlink mutiplied by spacingScaleFactor. 
    /// </summary>
    float spacingScaleFactor = 10.0f;
    /// <summary>
    /// A local member used to keep track of time since last shuffle.
    /// </summary>
    float time = 0;
    /// <summary>
    /// A list of objects that need to shuffle around (fly).
    /// </summary>
    List<GameObject> toFly;
    /// <summary>
    /// A list of 3d positions corresponding to the 3d objects indicating where the objects are flying to.  
    /// </summary>
    List<Vector3> flyingTo;
    /// <summary>
    /// Sets the speed at which the objects should shuffle (fly) around. 
    /// </summary>
    public float flyingSpeed = 2;
    public GameObject camera;
    /// <summary>
    /// Sets the number of obstacles to be spawned.
    /// </summary>
    [Range(0, 10)]
    public int noOfObstacles = 0;
    GameObject ObstacleSpawner;
    ObstacleSpawnerController osc;
    /// <summary>
    /// Enable or disable debugging for the spawner.
    /// </summary>
    public bool SSDebugLogging = false;

    /// <summary>
    /// Monobehaviour start member. Called once in the beginning. 
    /// </summary>
    void Start()
    {
        
        if (Application.isEditor && !Application.isPlaying)
        {
            //Placeholder
        }

        if (Application.isPlaying)
        {
            ///Increment to accomodate the addition of the calibration round. 
            spawnerNumber++;

			///Set the minimum and maximum spawning height from the remote. 
			minHeight = gameParametersContainer.gameParam.minHeight;
			maxHeight = gameParametersContainer.gameParam.maxHeight;
			if (maxHeight < minHeight)
				maxHeight = minHeight;
            ///Create the height based region divisions.
            heightDivision1 = minHeight + (maxHeight - minHeight) / 3;
            heightDivision2 = minHeight + 2 * (maxHeight - minHeight) / 3;

            ///Find the ObstacleSpawner and spawn the required number of obstacles.
            ObstacleSpawner = GameObject.Find("ObstacleSpawner");
            osc = ObstacleSpawner.GetComponent<ObstacleSpawnerController>();
            //osc.spawnObstacles(noOfObstacles);
            osc.spawnObstacles(gameParametersContainer.gameParam.No_of_obstacles[spawnerNumber]);
            tempFloatingEffect = floatingEffect;
            ///Initialize all the necessary lists.
            toFly = new List<GameObject>();
            flyingTo = new List<Vector3>();
            existingObjs = new List<Vector2>();
            preFill();
            ///Load all the alphabets prefabs into the alphabets list. 
            GameObject[] alphabets = Resources.LoadAll<GameObject>("Spawner Prefabs");
            gameObjects = new List<GameObject>();
            GameObject Parent = GameObject.Find("Parent");

            foreach (Transform go in Parent.transform)
            {
                if (go.gameObject.name != "SendInput")
                    go.gameObject.SetActive(false);
            }
            
            ///Assign the bitmap of the substring (to indicate which letters of the solution are displayed and which are not) 
            ///based on which level of the puzzle we are at. 
            int[] bms;
            if (spawnerNumber == 0)
            {
                //Debug.Log("Calibration round");
                bms = gameParametersContainer.gameParam.bitMapSubString0;
            }
            else if (spawnerNumber == 1)
            {
                bms = gameParametersContainer.gameParam.bitMapSubString1;
            }
            else if (spawnerNumber == 2)
            {
                bms = gameParametersContainer.gameParam.bitMapSubString2;
            }else //if (spawnerNumber == 3)
            {                
                bms = gameParametersContainer.gameParam.bitMapSubString3;
            }
            ///Extract the solution string and all the otherparameters from the static variable which holds 
            ///the data received form the network.
            solution = gameParametersContainer.gameParam.input[spawnerNumber];
            spawnInterval = gameParametersContainer.gameParam.SpawnInterval[spawnerNumber];
            spin = gameParametersContainer.gameParam.spin[spawnerNumber];
            alphabetsFaceUsersEye = gameParametersContainer.gameParam.AlphabetsFaceUser[spawnerNumber];
            if (gameParametersContainer.gameParam.repeatSolution[spawnerNumber])
                repetitionCount = 1;
            ///Scale the flyingSpeed and the rotation speed while setting it.
            flyingSpeed = gameParametersContainer.gameParam.FlyingSpeed / 25.0f;
            rotationSpeed = (gameParametersContainer.gameParam.RotationSpeed * 15.0f) / 50.0f;

            ///Add the solution gameobjects to the list.
            ///We add the the solution alphabets as many times the repititionCount indicates.
            ///(This is a little inefficient, a faster solution would be to duplicate the list and add
            ///or directly add as many times while inserting)
            for (int i = 0; i <= repetitionCount; i++)
                for (int k = 0; k < solution.Length; k++)
                {
                    ///We check with the bitmapstring if that alphabet is a part of the solution alphabet.
                    if (bms[k] == 0)
                        foreach (GameObject go in alphabets)
                        {
                            if (char.ToLower(go.name[0]) == char.ToLower(solution[k]))
                                if (repetition)
                                    gameObjects.Add(go);
                                else if (!gameObjects.Contains(go))
                                    gameObjects.Add(go);
                        }
                }

            ///Create a copy of the solution objects.
            SolutionGameObjects = new List<GameObject>(gameObjects);

            ///Create a list of nonSolution objects.
            List<GameObject> nonSolutionAlphabets = new List<GameObject>();
            foreach (GameObject go in alphabets)
                if (!gameObjects.Contains(go))
                    nonSolutionAlphabets.Add(go);

            ///We randomize it and add to the list if all other letters are to be spawned.
            if (spawnAllOtherAlphabets)
            {
                nonSolutionAlphabets = nonSolutionAlphabets.OrderBy(x => Random.value).ToList();
                foreach (GameObject go in nonSolutionAlphabets)
                    gameObjects.Add(go);
            }
            ///We add only the letters to be spawned otherwise, after checking it is not a 
            ///solution letter.
            else
            {
                foreach (GameObject go in nonSolutionAlphabets)
                    if (otherAlphabetsToSpawn.ToLower().Contains(char.ToLower(go.name[0])))
                        gameObjects.Add(go);
            }

            if (useCustomUserEyeLocation)
                centerPoint = userEyePoint;
            else
                centerPoint.Set(0f, 7.2f, 0f);
            
            ///Remove all the planes created in start.
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            
            ///Start of the acutal spawn code.
            if (gameObjects.Count > 0)
                ///If no restriction on spawning duplicates.
                if (spawnAtMultiplePlaces)
                {
                    for (int i = 0; i < noOfRows; i++)
                        for (int j = 0; j < noOfColumns; j++)
                        {
                            Vector3 spawnPos = transform.position;
                            spawnPos.x += i * (spawnerPlane.GetComponent<Renderer>().bounds.size.x + spacingx / spacingScaleFactor);
                            spawnPos.z += j * (spawnerPlane.GetComponent<Renderer>().bounds.size.z + spacingz / spacingScaleFactor);
                            if (Random.Range(0.0f, 100.0f) < spawnIntensity)
                            {
                                int randGOindex = Random.Range(0, gameObjects.Count);
                                float y_variation = Random.Range(minHeight, maxHeight);
                                spawnPos.y += y_variation;
                                spawnPos.y += gameObjects[randGOindex].GetComponent<Renderer>().bounds.size.y / 2;
                                GameObject spawnedGO = Instantiate(gameObjects[randGOindex], spawnPos, Quaternion.identity);
                                spawnedGO.transform.parent = transform;
                                Rigidbody rb = spawnedGO.GetComponent<Rigidbody>();
                                if (rb)
                                    rb.useGravity = gravity;
                                // Debug.Log("Spawning!");
                            }
                        }
                }
                //If each object is to be spawned only once. The Currently worked on one.
                else
                {
                    int region = 0;
                    for (int g = 0; g < gameObjects.Count; g++)
                    {
                        if (SolutionGameObjects.IndexOf(gameObjects[g]) < (SolutionGameObjects.Count / 3))
                            region = 0;
                        else if (SolutionGameObjects.IndexOf(gameObjects[g]) < (2 * SolutionGameObjects.Count / 3))
                            region = 1;
                        else
                            region = 2;

                        if(SSDebugLogging)
                            Debug.Log("Obj: " + g);
                        float randIntensity = Random.Range(0.0f, 100.0f);
                        if(SSDebugLogging)
                            Debug.Log("SpawnIntensity: " + randIntensity);
                        if (randIntensity < spawnIntensity)
                        {
                            //Debug.Log("Entering.");
                            int randx = Random.Range(0, noOfRows);
                            int randz = Random.Range(0, noOfColumns);
                            if(SSDebugLogging)
                                Debug.Log("Location generated: " + randx + "," + randz);
                            //Check if spot is already occupied.
                            if (existingObjs.Contains(new Vector2(randx, randz)))
                            {
                                if(SSDebugLogging)
                                    Debug.Log("Location not free!");
                                //If grid still has spots, check next available spot from current spot.
                                if (existingObjs.Count <= (noOfRows * noOfColumns))
                                {
                                    bool foundSpot = false;
                                    //Check for next available spot from the current spot.
                                    if(SSDebugLogging)
                                        Debug.Log("Checking next available spot from current spot.");
                                    for (int i = randx + 1; i < noOfRows; i++)
                                        for (int j = 0; j < noOfColumns; j++)
                                        {
                                            if (i == randx && j <= randz)
                                                continue;
                                            if (!existingObjs.Contains(new Vector2(i, j)) && !foundSpot)
                                            {
                                                spawn(i, j, g, region);
                                                foundSpot = true;
                                                if(SSDebugLogging)
                                                    Debug.Log("Found a spot");
                                            }
                                        }

                                    //Check for a spot from start to current spot if spot not already found.
                                    if (!foundSpot)
                                    {
                                        if(SSDebugLogging)
                                            Debug.Log("Didnt find a spot, trying from start.");
                                        for (int i = 0; i <= randx; i++)
                                            for (int j = 0; j < noOfColumns; j++)
                                                if (!existingObjs.Contains(new Vector2(i, j)) && !foundSpot)
                                                {
                                                    spawn(i, j, g, region);
                                                    foundSpot = true;
                                                    if(SSDebugLogging)
                                                        Debug.Log("Found a spot");
                                                }
                                    }
                                }
                            }
                            //Current spot was free.
                            else
                            {
                                if(SSDebugLogging)
                                    Debug.Log("Location free!");
                                spawn(randx, randz, g, region);
                            }
                        }
                    }
                }
            else
                if(SSDebugLogging)
                    Debug.Log("No gameobjects specified for spawner!");
        }
    }

    /// <summary>
    /// Monobehaviour start member. Called once per frame.
    /// </summary>
    void Update()
    {
        ///For displaying in editor, no gameplay code in here.       
        if (Application.isEditor && !Application.isPlaying)
        {
            ///Delete all existing planes before regenrating them.
            while (transform.childCount != 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            existingObjs = new List<Vector2>();
            preFill();
            for (int i = 0; i < noOfRows; i++)
                for (int j = 0; j < noOfColumns; j++)
                {
                    if (!existingObjs.Contains(new Vector2(i, j)))
                    {
                        Vector3 spawnPos = transform.position;
                        spawnPos.x += i * (spawnerPlane.GetComponent<Renderer>().bounds.size.x + spacingx / spacingScaleFactor);
                        spawnPos.z += j * (spawnerPlane.GetComponent<Renderer>().bounds.size.z + spacingz / spacingScaleFactor);
                        spawnPos.y += minHeight;
                        GameObject spawnedPlanes = Instantiate(spawnerPlane, spawnPos, Quaternion.identity);
                        spawnedPlanes.transform.parent = transform;
                    }
                }
            if (maxHeight != 0)
                for (int i = 0; i < noOfRows; i++)
                    for (int j = 0; j < noOfColumns; j++)
                    {
                        if (!existingObjs.Contains(new Vector2(i, j)))
                        {
                            Vector3 spawnPos = transform.position;
                            spawnPos.x += i * (spawnerPlane.GetComponent<Renderer>().bounds.size.x + spacingx / spacingScaleFactor);
                            spawnPos.z += j * (spawnerPlane.GetComponent<Renderer>().bounds.size.z + spacingz / spacingScaleFactor);
                            spawnPos.y += maxHeight;
                            GameObject spawnedPlanes = Instantiate(spawnerPlane, spawnPos, Quaternion.identity);
                            spawnedPlanes.transform.parent = transform;
                        }
                    }
        }

        ///Actual game code.
        if (Application.isPlaying)
        {   ///Manage floating effect, spin and lookAt camera effect.        
            foreach (Transform child in transform)
            {
                SpawnerObjectController soc = child.gameObject.GetComponent<SpawnerObjectController>();
                soc.spin = spin;
                if (floatingEffect && !soc.floatingEffect)
                {
                    soc.startFloating();
                }
                else if (!floatingEffect && soc.floatingEffect)
                    soc.floatingEffect = floatingEffect;
                if (alphabetsFaceUsersEye)
                {
                    child.gameObject.transform.LookAt(camera.transform.position);
                    child.gameObject.transform.Rotate(0, 180, 0, Space.Self);
                }
            }

            ///Manage respawn after falling
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("InteractionObject"))
                if (go.GetComponent<SpawnerObjectController>() && go.transform.position.y - 0.1 <= (transform.position.y + go.GetComponent<Renderer>().bounds.size.y / 2)
                    || go.transform.position.y - 0.1 <= (transform.position.y + go.GetComponent<Renderer>().bounds.size.x / 2)
                    || go.transform.position.y - 0.1 <= (transform.position.y + go.GetComponent<Renderer>().bounds.size.z / 2))
                {
                    Debug.Log("fallen");
                    string name = go.name;
                    SpawnerObjectController socloc = go.GetComponent<SpawnerObjectController>();
                    //existingObjs.Remove(socloc.pos);
                    GameObject.Destroy(go);
                    for (int g = 0; g < gameObjects.Count; g++)
                        if (char.ToLower(gameObjects[g].name[0]) == char.ToLower(name[0]))
                        {
                            if(SSDebugLogging)
                                Debug.Log("Obj: " + g);

                            GameObject temp = new GameObject();
                            foreach(GameObject gobj in SolutionGameObjects)
                            {
                                if (gobj.name[0] == gameObjects[g].name[0])
                                {
                                    temp = gobj;
                                    break;
                                }                                    
                            }
                            ///Note: The correct spot is not chosen for repeating letters. 
                            int region = 0;
                            if (SolutionGameObjects.IndexOf(temp) < (SolutionGameObjects.Count / 3))
                                region = 0;
                            else if (SolutionGameObjects.IndexOf(temp) < (2 * SolutionGameObjects.Count / 3))
                                region = 1;
                            else
                                region = 2;

                            if (true)
                            {
                                ///We start searching for free spots from a random spot in the grid 
                                ///so the spawner doesnt become repetitive 
                                int randx = Random.Range(0, noOfRows);
                                int randz = Random.Range(0, noOfColumns);
                                if(SSDebugLogging)
                                    Debug.Log("Location generated: " + randx + "," + randz);

                                ///Check if the random spot is already occupied.
                                if (existingObjs.Contains(new Vector2(randx, randz)))
                                {
                                    if(SSDebugLogging)
                                        Debug.Log("Location not free!");

                                    ///If grid still has spots, check next available spot from current spot.
                                    if (existingObjs.Count <= (noOfRows * noOfColumns))
                                    {
                                        bool foundSpot = false;
                                        //Check for next available spot from the current spot.
                                        if(SSDebugLogging)
                                            Debug.Log("Checking next available spot from current spot.");
                                        for (int i = randx + 1; i < noOfRows; i++)
                                            for (int j = 0; j < noOfColumns; j++)
                                            {
                                                if (i == randx && j <= randz)
                                                    continue;
                                                if (!existingObjs.Contains(new Vector2(i, j)) && !foundSpot)
                                                {
                                                    spawn(i, j, g, region);
                                                    foundSpot = true;
                                                    if(SSDebugLogging)
                                                        Debug.Log("Found a spot");
                                                }
                                            }

                                        //Check for a spot from start to current spot if a spot is not already found.
                                        if (!foundSpot)
                                        {
                                            if(SSDebugLogging)
                                                Debug.Log("Didnt find a spot, trying from start.");
                                            for (int i = 0; i <= randx; i++)
                                                for (int j = 0; j < noOfColumns; j++)
                                                    if (!existingObjs.Contains(new Vector2(i, j)) && !foundSpot)
                                                    {
                                                        spawn(i, j, g, region);
                                                        foundSpot = true;
                                                        if(SSDebugLogging)
                                                            Debug.Log("Found a spot");
                                                    }
                                        }

                                    }
                                }

                                //Current spot was free.
                                else
                                {
                                    if(SSDebugLogging)
                                        Debug.Log("Location free!");
                                    spawn(randx, randz, g, region);
                                }
                            }
                        }
                }

            ///Check if there are any objects to be "flown", if none, increment interval interval.
            if (toFly.Count == 0)
            {
                time += Time.deltaTime;
                floatingEffect = tempFloatingEffect;
            }
            ///Perform letters flying action.
            else
            {
                for (int i = 0; i < toFly.Count; i++)
                {
                    toFly[i].transform.position = Vector3.MoveTowards(toFly[i].transform.position, flyingTo[i], flyingSpeed * Time.deltaTime);
                    if (toFly[i].transform.position == flyingTo[i])
                    {
                        toFly.Remove(toFly[i]);
                        flyingTo.Remove(flyingTo[i]);
                        i--;
                    }

                }
            }

            ///If time interval for letter shuffling has been reached, add the letters to the toFly list.
            ///We disable the floatingEffect during the flight and preserve its state, and it is restored
            ///after every flying action is performed.
            if (time >= spawnInterval)
            {
                tempFloatingEffect = floatingEffect;
                floatingEffect = false;
                foreach (Transform child in transform)
                {
                    toFly.Add(child.gameObject);
                    //child.gameObject.GetComponent<SpawnerObjectController>().floatingEffect = floatingEffect;
                }

                ///Determine the new location for the alphabets, to fly there to.
                existingObjs.Clear();
                for (int p = 0; p < toFly.Count; p++)
                {
                    //Debug.Log("Entering.");
                    int randx = Random.Range(0, noOfRows);
                    int randz = Random.Range(0, noOfColumns);
                    if(SSDebugLogging)
                        Debug.Log("Location generated: " + randx + "," + randz);
                    //Check if spot is already occupied.
                    if (existingObjs.Contains(new Vector2(randx, randz)))
                    {
                        if(SSDebugLogging)
                            Debug.Log("Location not free!");
                        //If grid still has spots, check next available spot from current spot.
                        if (existingObjs.Count <= (noOfRows * noOfColumns))
                        {
                            bool foundSpot = false;
                            //Check for next available spot from the current spot.
                            if(SSDebugLogging)
                                Debug.Log("Checking next available spot from current spot.");
                            for (int i = randx + 1; i < noOfRows; i++)
                                for (int j = 0; j < noOfColumns; j++)
                                {
                                    if (i == randx && j <= randz)
                                        continue;
                                    if (!existingObjs.Contains(new Vector3(i, j)) && !foundSpot)
                                    {
                                        flyingTo.Add(new Vector3(i, 0, j));
                                        foundSpot = true;
                                        if(SSDebugLogging)
                                            Debug.Log("Found a spot");
                                    }
                                }
                            //Check for a spot from start to current spot if spot not already found.
                            if (!foundSpot)
                            {
                                if(SSDebugLogging)
                                    Debug.Log("Didnt find a spot, trying from start.");
                                for (int i = 0; i <= randx; i++)
                                    for (int j = 0; j < noOfColumns; j++)
                                        if (!existingObjs.Contains(new Vector2(i, j)) && !foundSpot)
                                        {
                                            flyingTo.Add(new Vector3(i, 0, j));
                                            foundSpot = true;
                                            if(SSDebugLogging)
                                                Debug.Log("Found a spot");
                                        }
                            }
                        }
                    }
                    //Current spot was free.
                    else
                    {
                        if(SSDebugLogging)
                            Debug.Log("Location free!");
                        flyingTo.Add(new Vector3(randx, 0, randz));
                    }
                }

                ///Seperate the letters to be flown which are only solution letters.
                ///This is to be used to divide them into the three regions.
                List<GameObject> toFlySolnOnly = new List<GameObject>();
                foreach (GameObject gobj in toFly)
                    foreach (GameObject solns in SolutionGameObjects)
                    {
                        if (gobj.name[0] == solns.name[0])
                        {
                            toFlySolnOnly.Add(gobj);
                            break;
                        }
                    }               

                ///Process flying pos from row-col to 3d coords.
                for (int i = 0; i < flyingTo.Count; i++)
                {
                    Vector3 spawnPos = transform.position;
                    spawnPos.x += flyingTo[i].x * (spawnerPlane.GetComponent<Renderer>().bounds.size.x + spacingx / spacingScaleFactor);
                    spawnPos.z += flyingTo[i].z * (spawnerPlane.GetComponent<Renderer>().bounds.size.z + spacingz / spacingScaleFactor);
                    
                    if (toFlySolnOnly.IndexOf(toFly[i]) < (toFlySolnOnly.Count / 3))
                        {spawnPos.y += Random.Range(minHeight, heightDivision1);Debug.Log("Region: 1");}
                    else if (toFlySolnOnly.IndexOf(toFly[i]) < (2 * toFlySolnOnly.Count / 3))
                        {spawnPos.y += Random.Range(heightDivision1, heightDivision2);Debug.Log("Region: 2");}
                    else
                        {spawnPos.y += Random.Range(heightDivision2, maxHeight);Debug.Log("Region: 3");}
                    flyingTo[i] = spawnPos;
                }
                time = 0;
            }
        }
    }
    //TBD: Fix adding parameter spec in doxygen causing pdflatex to crash. 
    void spawn(int rowSpawnedAt, int colSpawnedAt, int gameObjectIndex, int region)
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x += rowSpawnedAt * (spawnerPlane.GetComponent<Renderer>().bounds.size.x + spacingx / spacingScaleFactor);
        spawnPos.z += colSpawnedAt * (spawnerPlane.GetComponent<Renderer>().bounds.size.z + spacingz / spacingScaleFactor);
        if(region == 0)
            {spawnPos.y += Random.Range(minHeight, heightDivision1);Debug.Log("Region: 1");}
        else if (region == 1)
            {spawnPos.y += Random.Range(heightDivision1, heightDivision2);Debug.Log("Region: 2");}
        else
            {spawnPos.y += Random.Range(heightDivision2, maxHeight);Debug.Log("Region: 3");}

        spawnPos.y += gameObjects[gameObjectIndex].GetComponent<Renderer>().bounds.size.y / 2;
        GameObject spawnedGO = Instantiate(gameObjects[gameObjectIndex], spawnPos, Quaternion.identity);
        spawnedGO.transform.parent = transform;

        if (alphabetsFaceUsersEye)
        {
            spawnedGO.transform.LookAt(camera.transform.position);
            spawnedGO.transform.Rotate(0, 180, 0, Space.Self);
            //spawnedGO.transform.Rotate(Vector3.up, -90);                                                
            //spawnedGO.transform.rotation.eulerAngles.Set(spawnedGO.transform.rotation.eulerAngles.x, spawnedGO.transform.rotation.eulerAngles.y+90,0);
            //spawnedGO.transform.localEulerAngles = (new Vector3(spawnedGO.transform.localEulerAngles.x, spawnedGO.transform.localEulerAngles.y + 180, spawnedGO.transform.localEulerAngles.z));
        }

        if (limitMaximumPitchChange)
        {
            if (spawnedGO.transform.eulerAngles.x > maximumPitchChange && spawnedGO.transform.eulerAngles.x < 180)
                spawnedGO.transform.eulerAngles = new Vector3(maximumPitchChange, spawnedGO.transform.eulerAngles.y, spawnedGO.transform.eulerAngles.z);
            else if ((360 - spawnedGO.transform.eulerAngles.x) > maximumPitchChange && (360 - spawnedGO.transform.eulerAngles.x) < 180)
                spawnedGO.transform.eulerAngles = new Vector3(-maximumPitchChange, spawnedGO.transform.eulerAngles.y, spawnedGO.transform.eulerAngles.z);
        }

        Rigidbody rb = spawnedGO.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.useGravity = gravity;
            rb.isKinematic = !gravity;
        }

        SpawnerObjectController fe = spawnedGO.AddComponent<SpawnerObjectController>();
        fe.frequency = frequency;
        fe.amplitude = height;
        fe.spin = spin;
        fe.degreesPerSecond = rotationSpeed;
        fe.floatingEffect = floatingEffect;
        fe.pos = new Vector2(rowSpawnedAt, colSpawnedAt);

        existingObjs.Add(new Vector2(rowSpawnedAt, colSpawnedAt));

    }

    /// <summary>
    ///Prefill populates the existingObjs list with the user space grid so that the objects dont spawn in the user's area.
    /// </summary>
    void preFill()
    {
        if (userArea > 0)
        {
            //Debug.Log("Entered Prefill");
            ///Use hashSet instead of lists to avoid duplicate entries.
            HashSet<int> rowNos = new HashSet<int>(), colNos = new HashSet<int>();
            if (noOfRows % 2 != 0)
                for (int i = 0; i < userArea; i++)
                {
                    rowNos.Add((noOfRows - 1) / 2 + i);
                    rowNos.Add((noOfRows - 1) / 2 - i);
                }
            else
                for (int i = 0; i < userArea; i++)
                {
                    rowNos.Add((noOfRows - 1) / 2 + 1 + i);
                    rowNos.Add((noOfRows - 1) / 2 - i);
                }
            //Debug.Log("rowNos count is: " + rowNos.Count);
            //foreach (int item in rowNos) { Debug.Log("Row positions generated in preFill: " + item); }

            if (noOfColumns % 2 != 0)
                for (int i = 0; i < userArea; i++)
                {
                    colNos.Add((noOfColumns - 1) / 2 + i);
                    colNos.Add((noOfColumns - 1) / 2 - i);
                }
            else
                for (int i = 0; i < userArea; i++)
                {
                    colNos.Add((noOfColumns - 1) / 2 + 1 + i);
                    colNos.Add((noOfColumns - 1) / 2 - i);
                }

            //Debug.Log("colNos count is: " + colNos.Count);
            //foreach (int item in colNos) { Debug.Log("Col positions generated in preFill: " + item); }

            foreach (int rowNo in rowNos)
                foreach (int colNo in colNos)
                    if (!existingObjs.Contains(new Vector2(rowNo, colNo)))
                        existingObjs.Add(new Vector2(rowNo, colNo));



            //foreach (Vector2 item in existingObjList) { Debug.Log("Positions generated in preFill: " + item.x + "," + item.y); }
        }

    }

    //TBD: adding parameter spec in doxygen causes pdflatex to crash. 
    public void remove(Vector2 pos)
    {
        existingObjs.Remove(pos);
    }

    //IEnumerator waitAndFly(float waitTime, int rowNo, int colNo, GameObject go)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    go.transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    //}
}
//[CustomEditor(typeof(SpawnerSettings))]
//public class SpawnerSettingsEditor : Editor
//{
//    SpawnerSettings script;
//    void OnEnable()
//    {
//        script = (SpawnerSettings)target;
//    }
//    public override void OnInspectorGUI()
//    {
//        EditorGUI.indentLevel++;
//        if (script.spawnAtMultiplePlaces)
//        {
//            script.spawnIntensity = EditorGUILayout.Slider("Percentage chance of spawning at each location", script.spawnIntensity, 1, 100);
//        }

//    }

//}



