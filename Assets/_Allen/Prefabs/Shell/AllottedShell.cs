using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllottedShell : MonoBehaviour
{
    [SerializeField] private GameObject mShell;
    [SerializeField] private int mAmmoCount;
    [Space]
    [SerializeField] private GameObject htbSlot;

    public int ammo
    {
        get
        {
            return mAmmoCount;
        }
    }

    public GameObject hotbarSlot
    {
        get { return htbSlot; }
    }

    // Only Call Once for each respawn
    public void SetAmmo(int value)
    {
        mAmmoCount = value;
    }

    public void RemoveAmmo()
    {
        if (mAmmoCount <= 0) return;

        mAmmoCount--;
    }

    public GameObject shell
    {
        get
        {
            return mShell;
        }
    }

}
