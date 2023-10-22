using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    private List<GameObject> slots = new List<GameObject>();
    private List<GameObject> spawnedSlots = new List<GameObject>();

    private int selectedIndex = 0;

    public int Index
    {
        get
        {
            return selectedIndex;
        }
        private set
        {
            selectedIndex = value;
        }
    }

    public void SetAmmoText(string ammoText)
    {
        spawnedSlots[Index].GetComponent<Slot>().SetAmmoText(ammoText);
    }

    public void Init(List<AllottedShell> allottedShells)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].GetComponent<Slot>().Init((i + 1).ToString(), $"{allottedShells[i].ammo}");
            spawnedSlots.Add(Instantiate(slots[i], transform));
        }      

        SelectSlot(1);
    }    

    public void SelectSlot(int index)
    {
        Index = index - 1;

        for(int i = 0; i < slots.Count; i++)
        {
            if (i == Index)
            {
                spawnedSlots[i].GetComponent<Slot>().Select();
            }
            else
            {
                spawnedSlots[i].GetComponent<Slot>().Deselect();
            }
        }
    }

    public void InitializeSlots(List<GameObject> slotsToAdd)
    {
        slots = slotsToAdd;
    }

}
