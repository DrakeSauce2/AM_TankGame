using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float turnSpeed;

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

