using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] GameObject healthBarPrefab;
    HealthComponent healthBarComponent;

    [Space]

    [SerializeField] private Armor armorPlates;

    private void Awake()
    {
        
    }
}
