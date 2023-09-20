using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float turnSpeed;

    private Rigidbody rBody;
    private Camera cam;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float speed = moveSpeed * 10f;

        if (GetInputDirection().z != 0)
        {
            rBody.AddForce(transform.forward * GetInputDirection().z * speed * Time.deltaTime);
        }

        if (GetInputDirection().x != 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(
                new Vector3(Input.GetAxis("Horizontal"), 0, 0), Vector3.up), Time.deltaTime * turnSpeed);
        }

    }

    private Vector3 GetInputDirection()
    {
        return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

}

