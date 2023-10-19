using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyNumText;
    [SerializeField] private TextMeshProUGUI ammoCount;
    [SerializeField] private GameObject fade;

    public void Init(string keyText, string ammoText)
    {
        keyNumText.text = keyText;
        ammoCount.text = ammoText;
    }

    public void SetAmmoText(string ammoText) 
    {
        ammoCount.text = ammoText;
    }

    public void Select()
    {
        fade.SetActive(false);
    }

    public void Deselect()
    {
        fade.SetActive(true);
    }

}
