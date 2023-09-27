using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField] [Range(10, 1000)] private float mThickness; // In Millimeters

    [Space]

    [SerializeField] [Range(0, 100)] private float keResistance;
    [SerializeField] [Range(0, 100)] private float heResistance;

    private float health = 100f;
    private float angle;

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

    public void CalculateDamage(Transform incomingObject, Shell shell)
    {
        Debug.Log($"Armor Hit!");
    }
}
