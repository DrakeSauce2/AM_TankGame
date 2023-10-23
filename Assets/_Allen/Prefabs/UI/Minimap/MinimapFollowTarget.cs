using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollowTarget : MonoBehaviour
{
    [SerializeField] GameObject objectIcon;
    GameObject spawnedIcon;

    private Transform followTarget;

    public void Init(Transform followTarget)
    {
        this.followTarget = followTarget;

        //spawnedIcon = Instantiate(objectIcon, FindObjectOfType<Canvas>().transform);
        spawnedIcon = Instantiate(objectIcon, Minimap.Instance.PlayerMinimap);

    }

    public void Deconstruct()
    {
        Destroy(spawnedIcon);
    }

    private void Update()
    {
        if (spawnedIcon == null) return;

        Vector3 screenPoint = Minimap.Instance.MinimapCamera.WorldToScreenPoint(followTarget.localPosition);
        screenPoint.z = 0;
        spawnedIcon.transform.localPosition = screenPoint - Minimap.Instance.Offset;
    }
}
