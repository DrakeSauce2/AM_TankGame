using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpScript : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;

    public Transform playerPos;
    public float scale;

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            
            ZipLine();
        }
    }

    public void ZipLine()
    {
        if (playerPos.position != endPos.position)
        {
            playerPos.position = Vector3.Lerp(playerPos.position, endPos.position, Time.deltaTime * scale);
        }
    }

}
