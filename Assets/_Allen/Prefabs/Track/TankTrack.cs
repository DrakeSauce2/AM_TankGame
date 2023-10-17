using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTrack : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheels;
    [SerializeField] private Transform[] wheelMeshs;
    
    private float acceleration;
    private Vector3 wheelPosition;
    private Quaternion wheelRotation;

    private void Update()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);

            wheelMeshs[i].position = wheelPosition;
            wheelMeshs[i].rotation = wheelRotation;
        }
    }

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
