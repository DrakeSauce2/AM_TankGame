using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValueGauge : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI healthAmountText;

    public void SetValue(float val, float maxVal)
    {
        slider.value = val / maxVal;
        healthAmountText.text = $"{val.ToString("F0")} / {maxVal.ToString("F0")}";
    }
}