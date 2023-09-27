using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Transform barrelEnd;

    [Space]
   
    [SerializeField] private GameObject shellPrefab;
    private Shell shell;

    private void Awake()
    {
        shell = shellPrefab.GetComponent<Shell>();
    }

    public void Shoot()
    {
        Instantiate(shellPrefab, barrelEnd.position, barrelEnd.rotation);
    }
}
