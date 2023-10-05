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
    [Space(10)]
    [SerializeField] private ParticleSystem impactParticle;

    private Rigidbody rBody;
    private Vector3 startPos;
    private Vector3 endPos;

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

        endPos = transform.position;
        mDistanceTravelled = GetDistanceTravelled();

        Quaternion targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        transform.rotation = targetRotation;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Vector3 entryPoint = transform.position;

        Instantiate(impactParticle, entryPoint, Quaternion.identity);

        if (collision.collider.CompareTag("Armor"))
        {

            if (collision.gameObject.GetComponent<Armor>())
                CalculatePenetration(collision);
        }


        Destroy(gameObject);
    }

    private float GetDistanceTravelled()
    {
        return Vector3.Distance(startPos, endPos);
    }

    #region Calculation Functions

    private bool CalculatePenetration(Collision collision)
    {
        Armor armor = collision.gameObject.GetComponent<Armor>();

        float impactAngle = CalculateImpactAngle(collision.contacts[0].normal, transform.position);
        float effectiveThickness = CalculateArmorThickness(armor, impactAngle);

        Debug.Log($"Impact Angle: {impactAngle}, Effective Thickness: {effectiveThickness}");

        if (impactAngle > mRicochetAngle)
        {
            // ricochet the shell and do something
            Debug.Log("Shell Ricochet!");
            return false;
        }

        if (penetration > effectiveThickness)
        {
            collision.gameObject.GetComponent<Armor>().CalculateDamage(transform, this);
            
            return true;
        } 
        
        //Should try something else instead of doing this
        return false;
    }

    private float CalculateImpactAngle(Vector3 from, Vector3 to)
    {
        float impactAngle = Vector3.Angle(from, to);
        impactAngle -= 180;
        impactAngle = Mathf.Abs(impactAngle);

        return impactAngle;
    }

    private float CalculateArmorThickness(Armor armor, float impactAngle)
    {
        float effectiveThickness = armor.thickness / Mathf.Sin(impactAngle);
        effectiveThickness = Mathf.Abs(effectiveThickness);
        
        return effectiveThickness;
    }

    private float CalculateFalloff()
    {
        return 0;
    }

    #endregion

}
