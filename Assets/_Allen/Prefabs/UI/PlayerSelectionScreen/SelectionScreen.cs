using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionScreen : MonoBehaviour
{
    [SerializeField] private GameObject cameraRig;
    [Space]
    [SerializeField] private Transform playerSpawnPoint;
    [Space]
    [SerializeField] private Transform vehicleSlotAttachPoint;
    [SerializeField] private Transform vehicleSlotAmmunitionAttachPoint;
    [Space]
    [SerializeField] private List<GameObject> vehicleSlots = new List<GameObject>();
    [Space]
    [SerializeField] private List<GameObject> spawnedVehicleSlots = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedVehicleAmmunitionSlots = new List<GameObject>();
    [Space]
    [SerializeField] private GameObject selectedVehicle = null;

    private void Awake()
    {
        InstantiateVehicleSlots();
    }

    public void SpawnPlayer()
    {
        if (selectedVehicle == null) return; 

        GameObject player = Instantiate(selectedVehicle.GetComponent<SelectionVehicle>().Vehicle, playerSpawnPoint.position, Quaternion.identity);
        Instantiate(cameraRig, playerSpawnPoint.position, Quaternion.identity);

        List<AllottedShell> shells = new List<AllottedShell>();
        //shells = GetAllotedShells(spawnedVehicleAmmunitionSlots);

        //player.GetComponent<Player>().Init(shells);
        player.GetComponent<Player>().Init(GetAllotedShells(spawnedVehicleAmmunitionSlots));

        gameObject.SetActive(false);
    }

    #region

    private void SelectVehicle(SelectionVehicle selection)
    {
        if (selection.gameObject == selectedVehicle) return;

        if (selectedVehicle != null)
        {
            DeselectVehicle();
        }

        spawnedVehicleAmmunitionSlots = selection.InstantiateAmmunitionSlots(vehicleSlotAmmunitionAttachPoint);

        selectedVehicle = selection.gameObject;
    }

    private void DeselectVehicle()
    {
        foreach (GameObject ammoSlot in spawnedVehicleAmmunitionSlots)
        {
            spawnedVehicleAmmunitionSlots.Remove(ammoSlot);
            Destroy(ammoSlot);
        }
    }

    private void InstantiateVehicleSlots()
    {
        foreach (GameObject vehicleSlot in vehicleSlots)
        {
            spawnedVehicleSlots.Add(Instantiate(vehicleSlot, vehicleSlotAttachPoint));
            SelectionVehicle vehicle = spawnedVehicleSlots[spawnedVehicleSlots.Count - 1].GetComponent<SelectionVehicle>();
            vehicle.Init(() => SelectVehicle(vehicle));
        }
    }

    private List<AllottedShell> GetAllotedShells(List<GameObject> shellObjs)
    {
        List<AllottedShell> shells = new List<AllottedShell>();

        foreach (GameObject shellObj in shellObjs)
        {
            // Assign ammo from screen to this new list

            shellObj.GetComponent<AllottedShell>().SetAmmo(shellObj.GetComponent<SelectionScreenSlot>().GetAmmoCount());

            shells.Add(shellObj.GetComponent<AllottedShell>());
        }

        return shells;
    }

    #endregion 

}
