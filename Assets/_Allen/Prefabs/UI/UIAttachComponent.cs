using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAttachComponent : MonoBehaviour
{
    private Transform attachParentTransform;
    public void SetupAttachment(Transform attachParentTransform)
    {
        this.attachParentTransform = attachParentTransform;
    }

    private void Update()
    {
        if (CameraRig.Instance == null) return;

        transform.position = CameraRig.Instance.GetActiveCamera().WorldToScreenPoint(attachParentTransform.position);
    }

}