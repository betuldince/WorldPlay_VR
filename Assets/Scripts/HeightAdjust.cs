/// \file
/// \brief HeightAdjust is used to change the height of the play area as configured.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class changes the height of the play area as configured. 
/// </summary>
public class HeightAdjust : MonoBehaviour {
    public float minHeight = -15.6f, maxHeight = 100;
    /// <summary>
    /// Sets the height at which the ramp and all the objects should be placed at. 
    /// </summary>
    public float floorHeight = 100;    
    float offset = -15.6f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(floorHeight<= minHeight)
        {
            floorHeight = minHeight;
            transform.position = new Vector3(0.0f, floorHeight, 0.0f);
        }
        else if (floorHeight >= maxHeight)
        {
            floorHeight = maxHeight;
            transform.position = new Vector3(0.0f, floorHeight, 0.0f);
        }
        else
        {
            transform.position = new Vector3(0.0f, floorHeight, 0.0f);
        }
    }
}
