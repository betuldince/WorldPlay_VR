using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPhysics : MonoBehaviour {
   
    void SetPhysicsForSolutionBlock()
    {
        BoxCollider gameObjectBC = this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.transform.RotateAround(transform.position, Vector3.up, 90);

    }
   

    private void Start()
    {
        
            SetPhysicsForSolutionBlock();
       

    }
}
