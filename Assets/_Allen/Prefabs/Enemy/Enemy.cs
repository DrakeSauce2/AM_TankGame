using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Space]

    [SerializeField] private TextMeshProUGUI damageText;
    private Animator damageAnim;

    

    private MinimapFollowTarget minimapFollowTarget;

    private void Awake()
    {
        minimapFollowTarget = GetComponent<MinimapFollowTarget>();
        minimapFollowTarget.Init(transform);

        healthComponent = GetComponent<HealthComponent>();
        healthComponent.onTakenDamage += TookDamage;
        healthComponent.onHealthEmpty += StartDeath;
        healthComponent.onHealthChanged += HealthChanged;

        healthBar = Instantiate(healthBarPrefab, FindObjectOfType<Canvas>().transform);

        UIAttachComponent attachComp = healthBar.AddComponent<UIAttachComponent>();
        UIAttachComponent damageAttachComp = damageText.AddComponent<UIAttachComponent>();

        attachComp.SetupAttachment(healthBarAttachTransform);
        damageAttachComp.SetupAttachment(healthBarAttachTransform);

        damageAnim = damageText.GetComponent<Animator>();

        damageText.text = "";

        SetArmorOwner();
    }

    private void SetArmorOwner()
    {
        foreach (Armor armor in armorPlates)
        {
            armor.SetOwner(gameObject);
        }
    }

    private void HealthChanged(float currentHealth, float delta, float maxHealth)
    {
        healthBar.SetValue(currentHealth, maxHealth);

        damageText.text = delta.ToString();

        damageAnim.SetTrigger("TookDamage");
        //damageAnim.ResetTrigger("TookDamage");
    }

    private void StartDeath(float delta, float maxHealth)
    {
        minimapFollowTarget.Deconstruct();

        Instantiate(GameManager.Instance.DeadTankPrefab, transform.position, transform.rotation);

        Destroy(healthBar.gameObject);
        Destroy(damageText);
        Destroy(gameObject);
    }

    private void TookDamage(float currentHealth, float delta, float maxHealth)
    {
        
    }
}
