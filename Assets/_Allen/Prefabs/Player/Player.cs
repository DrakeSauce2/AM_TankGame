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

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void FixedUpdate()
    {       
        Move();
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

