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

    private bool isDestroyed = false;

    private void Update()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);

            wheelMeshs[i].position = wheelPosition;
            wheelMeshs[i].rotation = wheelRotation;
        }
    }

    private void OnDisable()
    {
        isDestroyed = true;
        foreach (WheelCollider wheel in wheels)
        {
            wheel.wheelDampingRate = 100000f;
        }
    }

    private void OnEnable()
    {
        isDestroyed = false;
        foreach (WheelCollider wheel in wheels)
        {
            wheel.wheelDampingRate = 10f;
        }
    }

    public void SetAcceleration(float accel)
    {
        acceleration = accel;
    }

    public void Accelerate(int dir)
    {
        if (isDestroyed == true) return;

        foreach (WheelCollider wheel in wheels)
        {
            wheel.motorTorque = acceleration * dir;
        }
    }

    public void Brake(float brakeTorque)
    {
        if (isDestroyed == true) return;

        foreach (WheelCollider wheel in wheels)
        {
            wheel.brakeTorque = brakeTorque;
        }
    }

}
