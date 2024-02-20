using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterSelection  : MonoBehaviour
{
    // Start is called before the first frame update
    public int count; 
     
    /// <summary>
    /// The solution string. Attached to \link SpawnerSettings::solutionString \endlink. 
    /// </summary>
    public string[] input;
    public string[] inputHint;


    /// THe number of letters in the solution string which are to be hidden. 
    /// </summary>
    public int[] No_of_blanks;
    /// <summary>
    /// The number of obstacles for each round.
    /// </summary>

    /// <summary>
    /// The interval between the spawned objects disappearing and reappearing. Attached to \link SpawnerSettings::spawnInterval \endlink.
    /// </summary>
    public int[] SpawnInterval;
    /// <summary>
    /// If the spawned objects always face the user's eye always or not. Attached to \link SpawnerSettings::alphabetsFaceUsersEye \endlink.
    /// </summary>
    public bool[] AlphabetsFaceUser;
    /// <summary>
    /// Enable or diable a spinning effect for the spawned objects. Attached to \link SpawnerSettings::spin \endlink.
    /// </summary>
    public bool[] spin;
    /// <summary>
    /// If the alphabets pertaining to the solving the puzzle are to be spawned twice. 
    /// </summary>
    public bool[] repeatSolution;
    /// <summary>
    /// Sets the speed at which the objects should shuffle (fly) around. Attached to \link SpawnerSettings::flyingSpeed \endlink.
    /// </summary>
    public int FlyingSpeed;
    /// <summary>
    /// Sets the speed at which the alphabets should rotate. Attached to \link SpawnerSettings::rotationSpeed \endlink.
    /// </summary>
    public int RotationSpeed;
    /// <summary>
    /// Represents the ten dgree difficulty of a word as denoted by the TwinWord Langauge Scoring API. 
    /// </summary>
    public int[] Difficulty;
    /// <summary>
 
 

}
