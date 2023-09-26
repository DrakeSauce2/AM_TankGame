using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{
    [SerializeField] private float mForce;
    [SerializeField] private float mMass;
    [SerializeField] private float mPen;

    private Rigidbody rBody;

    public float penetration
    {
        get
        {
            return mPen;
        }
    }

    public Vector3 velocity
    {
        get 
        {
            return rBody.velocity;
        }
    }

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();

        rBody.mass = mMass;

        rBody.AddForce(transform.forward * mForce);
    }

    private void Update()
    {
        transform.localEulerAngles = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Armor"))
        {
            Debug.Log($"Hit: {collision.collider.gameObject}");
        }

        Destroy(gameObject);
    }

}
