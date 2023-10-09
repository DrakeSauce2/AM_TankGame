using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankTrack : MonoBehaviour
{
    [SerializeField] private float thrust = 0f;
    private float maxSpeed;

    private Rigidbody rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rBody.velocity = Vector3.ClampMagnitude(rBody.velocity, maxSpeed);
    }

    public void SetMaxSpeed(float speed)
    {
        maxSpeed = speed;
    }

    #region Movement Directions

    public void Forward()
    {
        rBody.AddForce(transform.forward * thrust);
    }

    public void Backward()
    {
        rBody.AddForce(-transform.forward * thrust);
    }

    public void Right()
    {
        rBody.AddForce(transform.right * thrust);
    }

    public void Left()
    {
        rBody.AddForce(-transform.right * thrust);
    }

    #endregion

}
