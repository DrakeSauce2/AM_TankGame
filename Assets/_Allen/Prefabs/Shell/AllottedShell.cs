using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllottedShell : MonoBehaviour
{
    [SerializeField] private Shell mShell;
    [SerializeField] private int mAmmoCount;

    public int ammo
    {
        get
        {
            return mAmmoCount;
        }
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

    public Shell shell
    {
        get
        {
            return mShell;
        }
    }

}
