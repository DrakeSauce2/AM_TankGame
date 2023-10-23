using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using static HealthComponent;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera fpsCamera;

    [Space]

    [SerializeField] Transform cameraPivot;
    [SerializeField] Transform tankHead;
    [SerializeField] Transform tankBarrel;

    [Space]

    [SerializeField] private float maxSpeed;
    [SerializeField] private float turnSpeed;

    [Space]

    [SerializeField] private TankTrack leftTrack;
    [SerializeField] private TankTrack rightTrack;
    [SerializeField] private float tankAcceleration;
    [SerializeField] private float brakeTorque;
    [SerializeField] private float downForce;

    [Space]

    private Hotbar hotbar;
    [SerializeField] private List<AllottedShell> allottedShells = new List<AllottedShell>();
    private int ammo;

    [Space]

    [SerializeField] private Cannon cannon;
    [SerializeField] private Cannon machineGun;
    [SerializeField] private float shotForce;

    [Space]

    [SerializeField] private ValueGauge healthBarPrefab;
    HealthComponent healthComponent;

    [Space]

    [SerializeField] private List<Armor> armorPlates = new List<Armor>();
    [SerializeField] private List<TankComponent> components = new List<TankComponent>();
    [SerializeField] private float repairTime;
    [SerializeField] private bool isRepairing = false;

    [Space]

    [SerializeField] private AudioSource engineAudio;
    [SerializeField] private float engineAudioSmoothing;

    private MinimapFollowTarget minimapFollowTarget;
    private Rigidbody rBody;

    private IEnumerator repairCoroutine;

    public void Init(List<AllottedShell> allottedShells)
    {
        rBody = GetComponent<Rigidbody>();

        minimapFollowTarget = GetComponent<MinimapFollowTarget>();
        minimapFollowTarget.Init(transform);

        CameraRig.Instance.Init(fpsCamera);
        CameraRig.Instance.AddTankFollow(cameraPivot, tankHead, tankBarrel);

        cannon.Init(GameManager.Instance.ReloadReticle, GameManager.Instance.ReloadFill);

        this.allottedShells = allottedShells;

        InitializeHotBar();

        healthComponent = GetComponent<HealthComponent>();
        healthComponent.onTakenDamage += TookDamage;
        healthComponent.onHealthEmpty += StartDeath;
        healthComponent.onHealthChanged += HealthChanged;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        SetArmorOwner();
        SetComponentOwner();
        GameManager.Instance.SetPlayerTankComponents(components);

        leftTrack.SetAcceleration(tankAcceleration);
        rightTrack.SetAcceleration(tankAcceleration);

        repairCoroutine = StartRepair();

        ProcessSwitch(1);
    }

    private void InitializeHotBar()
    {
        hotbar = Instantiate(GameManager.Instance.HotBarPrefab, GameManager.Instance.HotBarAttachPoint).GetComponent<Hotbar>();

        List<GameObject> tempHotBarSlots = new List<GameObject>();
        foreach (AllottedShell slotToAdd in allottedShells)
        {
            tempHotBarSlots.Add(slotToAdd.hotbarSlot);
        }

        hotbar.InitializeSlots(tempHotBarSlots);

        hotbar.Init(allottedShells);
    }

    private void SetArmorOwner()
    {
        foreach (Armor armor in armorPlates)
        {
            armor.SetOwner(gameObject);
        }
    }

    private void SetComponentOwner()
    {
        foreach (TankComponent component in components)
        {
            component.Init(gameObject);
        }
    }   

    #region Health Events

    private void HealthChanged(float currentHealth, float delta, float maxHealth)
    {
        healthBarPrefab.SetValue(currentHealth, maxHealth);
    }

    private void StartDeath(float delta, float maxHealth)
    {
        minimapFollowTarget.Deconstruct();
    }

    private void TookDamage(float currentHealth, float delta, float maxHealth)
    {
        
    }

    #endregion

    float refSpeed = 0;
    private void Update()
    {
        CheckRepair();

        FireMainGun();       
        FireMachineGun();

        PitchEngineAudio();

        SwitchAmmo();
    }

    private void CheckRepair()
    {
        // Do the hold F to start repair thing
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (!isRepairing)
            {
                repairCoroutine = StartRepair();
                StartCoroutine(repairCoroutine);
            }
            else if (isRepairing)
            {
                StopCoroutine(repairCoroutine);
                isRepairing = false;
                GameManager.Instance.Reparing(false);
            }
        }
    }

    private IEnumerator StartRepair()
    {
        isRepairing = true;
        float time = repairTime;
        GameManager.Instance.Reparing(true);

        yield return new WaitForSeconds(time);

        foreach (TankComponent component in components)
        {
            if (!component.IsDestroyed) continue;

            component.Repair();
        }

        GameManager.Instance.Reparing(false);
        isRepairing = false;
    }

    private void PitchEngineAudio()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        float speed = Mathf.Clamp(input.magnitude, -1f, 1.25f);
        engineAudio.pitch = Mathf.SmoothDamp(engineAudio.pitch, speed, ref refSpeed, engineAudioSmoothing);
    }

    private void FixedUpdate()
    {
        
        if(!isRepairing)
            Move();
        

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

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ProcessSwitch(5);
        }
    }

    private void ProcessSwitch(int index)
    {
        if (index > allottedShells.Count) return;

        hotbar.SelectSlot(index);
        cannon.SwitchSelectedShell(allottedShells[index - 1].shell);
        ammo = allottedShells[index - 1].ammo;
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

            rBody.AddForceAtPosition(-cannon.BarrelEnd.forward * shotForce, cannon.BarrelEnd.position);

            allottedShells[hotbar.Index].RemoveAmmo();
            ammo = allottedShells[hotbar.Index].ammo;
            hotbar.SetAmmoText(ammo.ToString());
        }

    }

    private void FireMachineGun()
    {
        if (machineGun == null) return;

        if (Input.GetKey(KeyCode.Space))
        {
            machineGun.Shoot();         
        }
    }

    private void Move()
    {
        int forward = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        int horizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal"));

        CheckBrake(forward);

        leftTrack.Accelerate(forward);
        rightTrack.Accelerate(forward);

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

