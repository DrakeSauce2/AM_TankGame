using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SelectionScreenSlot : MonoBehaviour
{
    [SerializeField] private Slider ammoSlider;
    [SerializeField] private TextMeshProUGUI ammoText;
    [Space]
    [SerializeField] private int ammoCount;
    [SerializeField] private int maxAmmo;
    private AllottedShell allottedShell;

    private void Awake()
    {
        allottedShell = GetComponent<AllottedShell>();
        ammoSlider.minValue = 0;
        ammoSlider.maxValue = maxAmmo;
    }

    private void Update()
    {
        ammoCount = Mathf.RoundToInt(ammoSlider.value);
        ammoText.text = $"{ammoCount} / {maxAmmo}";
        allottedShell.SetAmmo(ammoCount);
    }

    public int GetAmmoCount()
    {
        return ammoCount;
    }

}
