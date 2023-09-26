using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Transform barrelEnd;

    [Space]
   
    [SerializeField] private GameObject shell;

    public void Shoot()
    {
        Instantiate(shell, barrelEnd.position, barrelEnd.rotation);
    }
}
