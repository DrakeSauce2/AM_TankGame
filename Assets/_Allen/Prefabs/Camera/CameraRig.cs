using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraRig : MonoBehaviour
{

    [SerializeField] private Transform followTarget;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    [SerializeField] private float followSpeed = 0.2f;
    [SerializeField] private LayerMask layerMask;

    [Space]

    [SerializeField] private Transform camPivot;
    [SerializeField] private Transform tankHead;
    [SerializeField] private Transform tankBarrel;
    [SerializeField] private float offset;

    [Space]

    [SerializeField] private float cameraSpeed;
    [SerializeField] private float headSpeed;
    private Camera cam;

    Vector3 refVel;

    float tankLookAngle = 0;
    float tankPivotAngle = 0;
    float camLookAngle = 0;
    float camPivotAngle = 0;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        FollowCam();

        RotateTankHead();
        RotateCamera();
    }

    private void FollowCam()
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, followTarget.position, ref cameraFollowVelocity, followSpeed);

        transform.position = targetPosition;
    }

    /*
    private void RotateTankHead()
    {
        Vector3 rotation;
        Quaternion targetRotation;
        

        tankLookAngle = tankLookAngle + (MouseInput().x * cameraSpeed);
        tankPivotAngle = tankPivotAngle - (MouseInput().y * cameraSpeed);
        tankPivotAngle = Mathf.Clamp(tankPivotAngle, -15, 12);

        rotation = Vector3.zero;
        rotation.y = tankLookAngle;
        targetRotation = Quaternion.Euler(rotation);
        tankHead.rotation = Quaternion.Slerp(tankHead.rotation, targetRotation, Time.deltaTime * headSpeed);
        //tankHead.localEulerAngles = Vector3.SmoothDamp(tankHead.localEulerAngles, targetRotation.eulerAngles, ref refVel, headSpeed);

        rotation = Vector3.zero;
        rotation.x = tankPivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        tankBarrel.localRotation = targetRotation; 

    }
    */

    Vector3 lookAtPoint;
    private void RotateTankHead()
    {
       
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, 10000f, layerMask))
        {
            lookAtPoint = hitInfo.point;
        }
        else
        {
            lookAtPoint = new Vector3(cam.transform.position.x + cam.transform.forward.x * 100f,
                                      cam.transform.position.y + cam.transform.forward.y * 100f,
                                      cam.transform.position.z + cam.transform.forward.z * 100f);
        }

        tankHead.LookAt(lookAtPoint, tankHead.up);
        tankHead.rotation = new Quaternion(0, tankHead.rotation.y, 0, tankHead.rotation.w);

        tankBarrel.LookAt(lookAtPoint, tankBarrel.up);
        tankBarrel.localRotation = new Quaternion(tankBarrel.localRotation.x + offset, tankBarrel.localRotation.y, tankBarrel.localRotation.z, tankBarrel.localRotation.w);

    }

    private void RotateCamera()
    {
        Vector3 rotationY;
        Vector3 rotationX;
        Quaternion targetRotationY;
        Quaternion targetRotationX;

        camLookAngle = camLookAngle + (MouseInput().x * cameraSpeed);
        camPivotAngle = camPivotAngle - (MouseInput().y * cameraSpeed);
        camPivotAngle = Mathf.Clamp(camPivotAngle, -45, 90);

        rotationY = Vector3.zero;
        rotationY.y = camLookAngle;
        targetRotationY = Quaternion.Euler(rotationY);
        transform.rotation = targetRotationY;

        rotationX = Vector3.zero;
        rotationX.x = camPivotAngle;
        targetRotationX = Quaternion.Euler(rotationX);
        camPivot.localRotation = targetRotationX;

    }

    private Vector2 MouseInput()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }



}
