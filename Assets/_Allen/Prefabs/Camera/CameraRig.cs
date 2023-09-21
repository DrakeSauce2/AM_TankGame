using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraRig : MonoBehaviour
{

    [SerializeField] private Transform followTarget;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    [SerializeField] private float followSpeed = 0.2f;

    [Space]

    [SerializeField] private Transform camPivot;
    [SerializeField] private Transform tankHead;
    [SerializeField] private Transform tankBarrel;

    [Space]

    [SerializeField] private float cameraSpeed;
    [SerializeField] private float headSpeed;

    Vector3 refVel;

    float tankLookAngle = 0;
    float tankPivotAngle = 0;
    float camLookAngle = 0;
    float camPivotAngle = 0;

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
