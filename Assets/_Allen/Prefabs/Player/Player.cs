using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float turnSpeed;

    [Space]

    [SerializeField] private Hotbar hotbar;

    private Rigidbody rBody;
    private Cannon cannon;



    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
        cannon = GetComponent<Cannon>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {       
        Move();

        FireMainGun();

        SwitchAmmo();
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

    private void Move()
    {
        float speed = moveSpeed * 10f;

        if (GetInputDirection().z != 0)
        {
            rBody.AddForce(transform.forward * Time.deltaTime * GetInputDirection().z * speed);
            rBody.velocity = Vector3.ClampMagnitude(rBody.velocity, maxSpeed);
        }

        if (GetInputDirection().x != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(
                transform.right * GetInputDirection().x * turnSpeed, transform.up), 1 * Time.deltaTime);
        }

    }

    private Vector3 GetInputDirection()
    {
        return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

}

