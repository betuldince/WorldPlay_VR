using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour {
    int currentNumberOfCollisions = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        //Check collision with any part of the iKinema model. 
        //if (collision.gameObject.name == "IKinemaMocapManSneakerLeft" || collision.gameObject.name == "IKinemaMocapManSneakerRight" || collision.gameObject.name == "IKinemaMocapManBody" )
        if(collision.gameObject.name == "LeftToeEnd" || collision.gameObject.name == "RightToeEnd")
        {
            currentNumberOfCollisions++;
            //Consider only the first simultaneous collision.
            if(currentNumberOfCollisions == 1 && Time.time > 0.2 && gameStateContainer.state >=1 && gameStateContainer.state <= 3)
            {
                Debug.Log("Collision Occured");
                gameStateContainer.collisions[gameStateContainer.state - 1] ++;
            }
                

        }
            

    }

    void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.name == "IKinemaMocapManSneakerLeft" || collision.gameObject.name == "IKinemaMocapManSneakerRight" || collision.gameObject.name == "IKinemaMocapManBody")
        if (collision.gameObject.name == "LeftToeEnd" || collision.gameObject.name == "RightToeEnd")
            currentNumberOfCollisions--;
    }
}
