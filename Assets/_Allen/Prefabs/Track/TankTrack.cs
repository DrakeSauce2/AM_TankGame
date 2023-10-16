using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTrack : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheels;
    private float acceleration;

    public void SetAcceleration(float accel)
    {
        acceleration = accel;
    }

    public void Accelerate(int dir)
    {
        foreach (WheelCollider wheel in wheels)
        {
            wheel.motorTorque = acceleration * dir;
        }
    }

    public void Brake(float brakeTorque)
    {
        foreach (WheelCollider wheel in wheels)
        {
            wheel.brakeTorque = brakeTorque;
        }
    }

}
