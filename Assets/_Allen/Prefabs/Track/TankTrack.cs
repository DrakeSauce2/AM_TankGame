using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankTrack : MonoBehaviour
{
    private float thrust = 0f;

    private Rigidbody rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {


        rBody.AddForce(transform.forward * thrust);
    }

    private void Input()
    {
        
    }

}
