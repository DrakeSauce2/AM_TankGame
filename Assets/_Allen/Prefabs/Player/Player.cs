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
    [SerializeField] private List<AllottedShell> allottedShells = new List<AllottedShell>();
    private int ammo;

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

        ammo = allottedShells[hotbar.Index].ammo;

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
            ProcessSwitch(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ProcessSwitch(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ProcessSwitch(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ProcessSwitch(4);
        }
    }

    private void ProcessSwitch(int index)
    {
        hotbar.SelectSlot(index);
        cannon.SwitchSelectedShell(allottedShells[index - 1].shell);
        ammo = allottedShells[hotbar.Index].ammo;
        hotbar.SetAmmoText(ammo.ToString());
    }

    private void FireMainGun()
    {
        if (!cannon)
        {
            Debug.LogError("Tank Is Missing Cannon Script");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !cannon.IsReloading && ammo > 0)
        {
            cannon.Shoot();
            allottedShells[hotbar.Index].RemoveAmmo();
            hotbar.SetAmmoText(ammo.ToString());
        }

    }

    private void FireMachineGun()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            machineGun.Shoot();
        }
    }

    private void NewMove()
    {
        int forward = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        int horizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal"));

        CheckBrake(forward);

        leftTrack.Accelerate(forward);
        rightTrack.Accelerate(forward);

        float turn = forward != 0 ? turnSpeed : turnSpeed / 4;
        rBody.AddRelativeTorque(Vector3.up * horizontal * turnSpeed * Time.fixedDeltaTime, ForceMode.Acceleration);

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

}

