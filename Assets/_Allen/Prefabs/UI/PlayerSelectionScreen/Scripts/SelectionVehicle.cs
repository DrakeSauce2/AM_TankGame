using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class SelectionVehicle : MonoBehaviour
{
    [SerializeField] private List<GameObject> availableShells;
    [SerializeField] private GameObject vehiclePrefab;
    private Button button;

    public GameObject Vehicle
    {
        get { return vehiclePrefab; }
    }

    public void Init(UnityEngine.Events.UnityAction call)
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(call);
    }

    public List<GameObject> InstantiateAmmunitionSlots(Transform attachPoint)
    {
        List<GameObject> slots = new List<GameObject>();

        foreach (GameObject slot in availableShells)
        {
            GameObject newSlot = Instantiate(slot, attachPoint);
            slots.Add(newSlot);
        }

        return slots;
    }

}
