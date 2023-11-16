using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationController : MonoBehaviour {
    public GameObject camera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("InteractionObject"))
        {
            SpawnerObjectController soc = go.GetComponent<SpawnerObjectController>();
            if(soc)
                if (soc.interacted)
                {
                    go.transform.LookAt(camera.transform.position);
                    go.transform.Rotate(0, 180, 0, Space.Self);
                }
            
        }

    }
}
