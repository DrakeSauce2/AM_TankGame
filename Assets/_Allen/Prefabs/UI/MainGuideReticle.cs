using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGuideReticle : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [Space]
    [SerializeField] private Transform reticle;
    [SerializeField] private float smoothSpeed;

    Vector3 refVel = Vector3.zero;

    private void Update()
    {
        reticle.position = Vector3.SmoothDamp(reticle.position, cam.WorldToScreenPoint(cam.transform.forward), ref refVel, smoothSpeed * Time.deltaTime);
        
    }
}
