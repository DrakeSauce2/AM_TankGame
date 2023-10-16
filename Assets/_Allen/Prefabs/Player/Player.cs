using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using static HealthComponent;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float turnSpeed;

    [Space]

    [SerializeField] private TankTrack leftTrack;
    [SerializeField] private TankTrack rightTrack;
    [SerializeField] private float tankAcceleration;
    [SerializeField] private float brakeTorque;
    [SerializeField] private float downForce;


    [Space]

    [SerializeField] private Hotbar hotbar;

    [Space]

    [SerializeField] private Cannon cannon;
    [SerializeField] private Cannon machineGun;

    [Space]

    [SerializeField] private ValueGauge healthBarPrefab;
    HealthComponent healthComponent;

    [Space]

    [SerializeField] private List<Armor> armorPlates = new List<Armor>();

    private Rigidbody rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();

        healthComponent = GetComponent<HealthComponent>();
        healthComponent.onTakenDamage += TookDamage;
        healthComponent.onHealthEmpty += StartDeath;
        healthComponent.onHealthChanged += HealthChanged;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        //SetArmorOwner();
        leftTrack.SetAcceleration(tankAcceleration);
        rightTrack.SetAcceleration(tankAcceleration);
    }

    private void SetArmorOwner()
    {
        foreach (Armor armor in armorPlates)
        {
            armor.SetOwner(gameObject);
        }
    }

    #region Health Events

    private void HealthChanged(float currentHealth, float delta, float maxHealth)
    {
        healthBarPrefab.SetValue(currentHealth, maxHealth);
    }

    private void StartDeath(float delta, float maxHealth)
    {
        
    }

    private void TookDamage(float currentHealth, float delta, float maxHealth)
    {
        
    }

    #endregion

    private void Update()
    {       
        FireMainGun();
        FireMachineGun();

        SwitchAmmo();
    }

    private void FixedUpdate()
    {
        NewMove();

        rBody.AddForce(-transform.up * downForce);
    }

    private void SwitchAmmo()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            hotbar.SelectSlot(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            hotbar.SelectSlot(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            hotbar.SelectSlot(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            hotbar.SelectSlot(4);
        }
    }

    private void FireMainGun()
    {
        if (!cannon)
        {
            Debug.LogError("Tank Is Missing Cannon Script");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            cannon.Shoot();
        }

    }

    private void FireMachineGun()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            machineGun.Shoot();
        }
    }

    private void Move()
    {
        float speed = moveSpeed * 10f;
        Vector3 inputDir = GetInputDirection();
        inputDir = inputDir.normalized;
        if (inputDir.magnitude != 0 )
        {
            //rBody.AddForce((transform.forward * inputDir.z + transform.right * inputDir.x) * Time.fixedDeltaTime * speed);
            rBody.AddForce(transform.forward * Time.deltaTime * GetInputDirection().z * speed);
            rBody.velocity = Vector3.ClampMagnitude(rBody.velocity, maxSpeed);
        }

        if(inputDir.x != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(
            transform.right * inputDir.x * turnSpeed, transform.up), 1 * Time.deltaTime);
        }
    }

    private void NewMove()
    {
        int forward = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        int horizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal"));

        CheckBrake(forward);

        if(horizontal != 0)
        {
            leftTrack.SetAcceleration(tankAcceleration * 4);
            rightTrack.SetAcceleration(tankAcceleration * 4);
        }
        else
        {
            leftTrack.SetAcceleration(tankAcceleration);
            rightTrack.SetAcceleration(tankAcceleration);
        }

        leftTrack.Accelerate(forward + horizontal);
        rightTrack.Accelerate(forward - horizontal);

        rBody.velocity = Vector3.ClampMagnitude(rBody.velocity, maxSpeed);
    }

    private void CheckBrake(int forward)
    {
        if (rBody.velocity.magnitude > 1 && forward < 0)
        {
            leftTrack.Brake(brakeTorque);
            rightTrack.Brake(brakeTorque);
        }
        else
        {
            leftTrack.Brake(0);
            rightTrack.Brake(0);
        }

    }

    private Vector3 GetInputDirection()
    {
        return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

}

