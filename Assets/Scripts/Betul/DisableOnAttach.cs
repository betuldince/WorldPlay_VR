using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnAttach : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisableScript()
    {
        gameObject.GetComponent<SpawnerObjectController>().enabled = false;
    }
}
