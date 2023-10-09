using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static HealthComponent;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ValueGauge healthBarPrefab;
    [SerializeField] private Transform healthBarAttachTransform;
    HealthComponent healthComponent;
    ValueGauge healthBar;

    [Space]

    [SerializeField] private List<Armor> armorPlates = new List<Armor>();

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.onTakenDamage += TookDamage;
        healthComponent.onHealthEmpty += StartDeath;
        healthComponent.onHealthChanged += HealthChanged;

        healthBar = Instantiate(healthBarPrefab, FindObjectOfType<Canvas>().transform);
        UIAttachComponent attachComp = healthBar.AddComponent<UIAttachComponent>();
        attachComp.SetupAttachment(healthBarAttachTransform);
    }

    private void SetArmorOwner()
    {
        foreach (Armor armor in armorPlates)
        {

        }
    }

    private void HealthChanged(float currentHealth, float delta, float maxHealth)
    {
        healthBar.SetValue(currentHealth, maxHealth);
    }

    private void StartDeath(float delta, float maxHealth)
    {
        
    }

    private void TookDamage(float currentHealth, float delta, float maxHealth)
    {
        
    }
}
