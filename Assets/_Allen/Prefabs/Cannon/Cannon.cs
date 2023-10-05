using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Transform barrelEnd;
    [SerializeField] private ParticleSystem shotParticle;

    [Space]
   
    [SerializeField] private GameObject shellPrefab;

    [Space]

    [SerializeField] private LayerMask layerMask;

    private TargetGuide guide;
    private Shell shell;

    private void Awake()
    {
        shell = shellPrefab.GetComponent<Shell>();
        guide = GetComponent<TargetGuide>();
    }

    private void Update()
    {
        if (Physics.Raycast(barrelEnd.position, barrelEnd.forward, out RaycastHit hitInfo, 10000f, layerMask))
        {
            Vector3 point = hitInfo.point;

            guide.SetCrosshair(Camera.main.WorldToScreenPoint(point));
        }       
    }

    public void Shoot()
    {
        Instantiate(shellPrefab, barrelEnd.position, barrelEnd.rotation);
        shotParticle.Play();
    }
}
