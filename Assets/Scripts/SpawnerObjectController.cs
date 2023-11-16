/// \file
/// \brief SpawnerObjectController is attached to define object properties as they are spawned and change them on the fly. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is attached to define object properties as they are spawned and change them on the fly.  
/// </summary>
public class SpawnerObjectController : MonoBehaviour
{
    /// <summary>
    /// Sets the rpm of the spin.
    /// </summary>
    public float degreesPerSecond = 15.0f;
    /// <summary>
    /// Sets the maximum distance the object moves from its default position for the floating effect.
    /// </summary>
    public float amplitude = 0.01f;
    /// <summary>
    /// Sets the number of times the object oscillates for the floating effect per second.
    /// </summary>
    public float frequency = 1f;
    /// <summary>
    /// Sets if the objects spins.
    /// </summary>
    public bool spin = false;
    /// <summary>
    /// Sets if the object has a ambient floating effect.
    /// </summary>
    public bool floatingEffect = false;
    /// <summary>
    /// If the object has been interacted with by the player. 
    /// </summary>
    public bool interacted = false;
    /// <summary>
    /// stores the inital position of the object.
    /// </summary>
    float startingPoint;
    /// <summary>
    /// indiactes the row and column the object is at. 
    /// </summary>
    public Vector2 pos;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    // Use this for initialization
    void Start()
    {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
        startingPoint = Random.Range(0.0f, 6.283f);
    }

    // Update is called once per frame
    void Update()
    {
        if (spin)
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down with a Sin().
        if (floatingEffect)
        {
            tempPos = posOffset;

            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency + startingPoint) * amplitude;

            transform.position = tempPos;
        }

    }

    /// <summary>
    /// Start floating effect on the object. 
    /// </summary>
    public void startFloating()
    {
        posOffset = transform.position;
        floatingEffect = true;
        startingPoint = Random.Range(0.0f, 6.283f);
    }
}
