using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyNumText;
    [SerializeField] private GameObject fade;

    public void Init(string text)
    {
        keyNumText.text = text;
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
