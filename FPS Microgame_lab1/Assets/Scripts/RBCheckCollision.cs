using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBCheckCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnControllerColliderHit(ControllerColliderHit hit){
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body!=null && !body.isKinematic)
        {
            body.velocity += hit.controller.velocity;
        }
    }
}
