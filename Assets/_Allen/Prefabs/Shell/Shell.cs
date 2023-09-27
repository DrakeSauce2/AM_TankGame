using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{

    #region Private Variables

    [SerializeField] private float mForce;
    [SerializeField] private float mMass;
    [SerializeField] private float mPen;
    [SerializeField] private float mRicochetAngle;
    [SerializeField] private float mDistanceTravelled;

    private Rigidbody rBody;
    private Vector3 startPos;

    #endregion

    #region Public Variables

    public float penetration
    {
        get
        {
            return mPen;
        }
    }

    public float mass
    {
        get 
        {
            return mMass; 
        }
    }

    public float force
    {
        get
        {
            return mForce;
        }
    }

    public Vector3 velocity
    {
        get 
        {
            return rBody.velocity;
        }
    }

    #endregion

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
        startPos = transform.position;

        rBody.mass = mMass;

        rBody.AddForce(transform.forward * mForce);
    }

    private void Update()
    {
        transform.localEulerAngles = velocity;

        Quaternion targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        transform.rotation = targetRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Armor"))
        {
            Debug.Log($"Hit: {collision.collider.gameObject}");

            if (collision.gameObject.GetComponent<Armor>())
            {
                Armor armor = collision.gameObject.GetComponent<Armor>();

                float impactAngle = Vector3.Angle(collision.contacts[0].normal, transform.position);
                impactAngle -= 180;
                impactAngle = Mathf.Abs(impactAngle);

                float effectiveThickness = armor.thickness / Mathf.Sin(impactAngle);
                effectiveThickness = Mathf.Abs(effectiveThickness);

                Debug.Log($"Impact Angle: {impactAngle}");
                Debug.Log($"Effective Thickness: {effectiveThickness}");

                if (penetration > effectiveThickness)
                {
                    collision.gameObject.GetComponent<Armor>().CalculateDamage(transform, this);
                }
                else
                {
                    Debug.Log("Deflection!");
                }
            }
        }

        

        Destroy(gameObject);
    }

}
