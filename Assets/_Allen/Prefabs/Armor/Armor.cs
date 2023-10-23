using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Armor : MonoBehaviour
{
    [SerializeField] [Range(10, 1000)] private float mThickness;

    [Space]

    [SerializeField] [Range(0, 1)] private float keResistance;
    [SerializeField] [Range(0, 1)] private float heResistance;
    private float angle;

    private HealthComponent ownerHealthComponent;

    public GameObject Owner
    {
        get;
        private set;
    }

    public float thickness
    {
        get
        {
            return mThickness;
        }
    }

    private void Awake()
    {
        angle = transform.rotation.x;
    }

    public void SetOwner(GameObject owner)
    {
        Owner = owner;
        ownerHealthComponent = owner.GetComponent<HealthComponent>();
    }

    public void CalculateDamage(Transform incomingObject, float damage, bool isKinetic)
    {
        if (isKinetic)
            damage -= damage * keResistance;
        else 
            damage -= damage * heResistance;

        Debug.Log(damage);

        ownerHealthComponent.ChangeHealth(-damage);
        //Owner.GetComponent<HealthComponent>().ChangeHealth(-10);
        Debug.Log("Object Hit");
    }
}
