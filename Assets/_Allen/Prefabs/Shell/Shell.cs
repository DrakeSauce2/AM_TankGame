using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ShellTypes
{
    KINETIC,
    ENERGY
}

[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{

    #region Private Variables

    [SerializeField] private float mForce;
    [Header("Kinetic Variables")]
    [SerializeField] private float mMass;
    [SerializeField] private float mPen;
    [SerializeField] private float mRicochetAngle;
    [Header("Energy Variables")]
    [SerializeField] private float blastRadius;
    [SerializeField] private float blastDamage;
    [SerializeField] private float blastPenetration;
    [Space(10)]
    [SerializeField] private ParticleSystem impactParticle;
    [Space(10)]
    [SerializeField] private ShellTypes shellType;

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

        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.localEulerAngles = velocity;

        endPos = transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        transform.rotation = targetRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        endPos = transform.position;

        Instantiate(impactParticle, endPos, Quaternion.identity);

        switch (shellType)
        {
            case ShellTypes.KINETIC:
                CalculateKineticPenetration(collision);
                break;
            case ShellTypes.ENERGY:
                CalculateEnergyPenetration();
                break;
            default:
                CalculateKineticPenetration(collision);
                break;
        }
        
        Destroy(gameObject);
    }

    private float GetDistanceTravelled()
    {
        return Vector3.Distance(startPos, endPos);
    }

    #region Calculation Functions

    public virtual void CalculateEnergyPenetration()
    {
        Collider[] colsInBlast = Physics.OverlapSphere(endPos, blastRadius);
        float cumulativeDamage = 0;

        foreach (Collider collider in colsInBlast)
        {
            Armor armor = collider.gameObject.GetComponent<Armor>();
            TankComponent component = collider.gameObject.GetComponent<TankComponent>();

            if (armor != null)
            {
                float totalDamage = Mathf.Abs(CalculateEnergyFalloff(endPos, collider.transform.position)) / colsInBlast.Length;

                collider.gameObject.GetComponent<Armor>().CalculateDamage(transform, totalDamage, false);
                cumulativeDamage += totalDamage;
                continue;
            }

            if (component != null)
            {
                float totalDamage = Mathf.Abs(CalculateEnergyFalloff(endPos, collider.transform.position)) / colsInBlast.Length;

                collider.gameObject.GetComponent<TankComponent>().CalculateDamage(totalDamage);
                cumulativeDamage += totalDamage;
                continue;
            }

        }

        Debug.Log(cumulativeDamage);
    }

    public virtual void CalculateKineticPenetration(Collision collision)
    {
        Armor armor = collision.gameObject.GetComponent<Armor>();
        TankComponent component = collision.gameObject.GetComponent<TankComponent>();

        if (armor != null)
        {

            // ^ Calculates After Check ^

            float impactAngle = CalculateImpactAngle(collision.contacts[0].normal, transform.position);
            float effectiveThickness = CalculateArmorThickness(armor, impactAngle);

            Debug.Log($"Impact Angle: {impactAngle}, Effective Thickness: {effectiveThickness}");

            float totalPenetration = CalculateKineticFalloff();
            Debug.Log(totalPenetration);

            /*
            if (impactAngle < mRicochetAngle)
            {
                // ricochet the shell and do something
                Debug.Log("Shell Ricochet!");
                return false;
            }
            */

            if (totalPenetration > effectiveThickness)
            {
                collision.gameObject.GetComponent<Armor>().CalculateDamage(transform, totalPenetration - effectiveThickness, true);
            }

            return;
        }

        if (component != null)
        {
            float totalPenetration = CalculateKineticFalloff();
            collision.gameObject.GetComponent<TankComponent>().CalculateDamage(totalPenetration);
        }
    }

    private float CalculateImpactAngle(Vector3 from, Vector3 to)
    {
        float impactAngle = Vector3.Angle(from, to);
        //impactAngle -= 180;
        impactAngle = Mathf.Abs(impactAngle);

        return impactAngle;
    }

    private float CalculateArmorThickness(Armor armor, float impactAngle)
    {
        float slope = Mathf.Sin(Mathf.Deg2Rad * impactAngle);
        if (slope == 0) return 0;
        slope = Mathf.Abs(slope);
        float effectiveThickness = armor.thickness / slope;

        return effectiveThickness;
    }

    public virtual float CalculateKineticFalloff()
    {
        float distance = GetDistanceTravelled();
        float tempPen = mPen;

        for (int i = Mathf.RoundToInt(distance); i > 0; i -= 5)
        {
            tempPen -= 2;
        }
        return tempPen;
    }

    private float CalculateEnergyFalloff(Vector3 shellHitPos, Vector3 colHitPos)
    {
        float distance = Vector3.Distance(shellHitPos, colHitPos);
        float tempDamage = blastDamage;

        for (int i = Mathf.RoundToInt(distance); i > 0; i -= 5)
        {
            tempDamage -= 2;
        }
        return tempDamage;
    }

    #endregion

}
