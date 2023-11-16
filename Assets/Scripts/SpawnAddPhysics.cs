using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAddPhysics : MonoBehaviour {
    

    void SetPhysicsForNonSolutionBlock()
    {
        BoxCollider gameObjectBC = this.gameObject.AddComponent<BoxCollider>();
        //Rigidbody gameObjectRB;
        //this.gameObject.transform.RotateAround(transform.position, Vector3.up, 270);
       


        //gameObjectRB = this.gameObject.AddComponent<Rigidbody>();

    }

    private void Start()
    {

        SetPhysicsForNonSolutionBlock();


    }
}
