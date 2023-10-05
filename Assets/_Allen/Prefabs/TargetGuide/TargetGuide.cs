using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGuide : MonoBehaviour
{
    [SerializeField] private RectTransform crossHair;

    public void SetCrosshair(Vector3 pos)
    {
        crossHair.position = pos;
    }
    
}
