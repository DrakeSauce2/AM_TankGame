using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankComponent : MonoBehaviour
{
    [SerializeField] MonoBehaviour componentAttachmentClass;
    [Space]
    [SerializeField] private float maxHealth;
    private float health;

    private bool isDestroyed = false;
    public bool IsDestroyed { get { return isDestroyed; } }

    Player player;
    Enemy enemy;

    public void Init(GameObject owner)
    {
        health = maxHealth;
        
        if (owner.GetComponent<Player>())
        {
            player = owner.GetComponent<Player>();
        }

        if (owner.GetComponent<Enemy>())
        {
            enemy = owner.GetComponent<Enemy>();
        }

    }

    public void CalculateDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !isDestroyed)
        {
            DisableComponent();
        }
    }

    public void DisableComponent()
    {
        componentAttachmentClass.enabled = false;
        isDestroyed = true;
    }

    public void Repair()
    {
        componentAttachmentClass.enabled = true;
        health = maxHealth;
        isDestroyed = false;
    }
}
