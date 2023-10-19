using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SelectionScreenSlot : MonoBehaviour
{
    [SerializeField] private Slider ammoSlider;
    [Space]
    [SerializeField] private int ammoCount;
    [SerializeField] private int minAmmo, maxAmmo;

    private void Awake()
    {
        ammoSlider.minValue = minAmmo;
        ammoSlider.maxValue = maxAmmo;
    }

    private void Update()
    {
        ammoCount = Mathf.RoundToInt(ammoSlider.value);
    }

    public void PushAmmo()
    {
        // Set ammo in player and hotbar
    }

}
