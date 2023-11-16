using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR.InteractionSystem.Sample;

public class InteractionController : MonoBehaviour
{
    // Start is called before the first frame update
     
    private Interactable interactable;
    private Throwable throwable;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        interactable = this.GetComponent<Interactable>();
        throwable= this.GetComponent<Throwable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 
    private void OnAttachedToHand(Hand hand)
    {
        SpawnerObjectController objectBeingInteracted = gameObject.GetComponent<SpawnerObjectController>();

        objectBeingInteracted.interacted = true;
        SpawnerSettings ss = GameObject.FindWithTag("Spawner").GetComponent<SpawnerSettings>();
        ss.remove(objectBeingInteracted.pos);
        objectBeingInteracted.spin = false;
        objectBeingInteracted.floatingEffect = false;
    }
    private void OnDetachedFromHand(Hand hand)
    {
        rb.useGravity = true;
        rb.isKinematic = false;
    }
     
}
