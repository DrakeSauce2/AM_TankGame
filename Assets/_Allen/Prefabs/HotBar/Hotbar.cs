using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    [SerializeField] private List<GameObject> slots = new List<GameObject>();

    private List<GameObject> spawnedSlots = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].GetComponent<Slot>().Init((i + 1).ToString());
            spawnedSlots.Add(Instantiate(slots[i], transform));
        }
        
        SelectSlot(1);
    }    

    public void SelectSlot(int index)
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if (i == index - 1)
            {
                spawnedSlots[i].GetComponent<Slot>().Select();
            }
            else
            {
                spawnedSlots[i].GetComponent<Slot>().Deselect();
            }
        }
    }

}
