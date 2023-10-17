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
    [SerializeField] private Vector3 offset;
    [SerializeField] private float headTurnSpeed;

    [Space]

    [SerializeField] private float cameraSpeed;
    [SerializeField] private float zoomSpeed;
    private Camera cam;

    Vector3 refVel;
    private bool zoom = false;

    float camLookAngle = 0;
    float camPivotAngle = 0;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            zoom = !zoom;
        }

        Zoom();
        
        FollowCam();

        RotateTankHead();
        RotateCamera();
    }

    private void Zoom()
    {
        if (zoom && cam.fieldOfView != 50)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 50, Time.deltaTime * zoomSpeed);
        }
        else if (!zoom && cam.fieldOfView != 75)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 75, Time.deltaTime * zoomSpeed);
        }       
    }

    private void FollowCam()
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, followTarget.position, ref cameraFollowVelocity, followSpeed);

        transform.position = targetPosition;
    }

    Vector3 lookAtPoint;
    private void RotateTankHead()
    {
       
        if (Physics.Raycast(cam.transform.position + offset, cam.transform.forward + offset, out RaycastHit hitInfo, 10000f, layerMask))
        {
            lookAtPoint = hitInfo.point;
        }
        else
        {
            lookAtPoint = new Vector3(cam.transform.position.x + offset.x + cam.transform.forward.x * 100f,
                                      cam.transform.position.y + offset.y + cam.transform.forward.y * 100f,
                                      cam.transform.position.z + offset.z + cam.transform.forward.z * 100f);
        }

        // New Code

        Vector3 up = tankHead.up;
        Vector3 directionToTarget = Vector3.ProjectOnPlane(lookAtPoint - tankHead.position, up);
        Quaternion turretTargetDirection = Quaternion.LookRotation(directionToTarget, up);

        Quaternion from = Quaternion.LookRotation(tankHead.forward, tankHead.up);
        tankHead.rotation = Quaternion.RotateTowards(from, turretTargetDirection, headTurnSpeed * Time.fixedDeltaTime);

        tankBarrel.LookAt(lookAtPoint, Vector3.up);

        tankBarrel.rotation = new Quaternion(ClampBarrelX(tankBarrel.rotation, 13),
                                             tankHead.rotation.y,
                                             tankHead.rotation.z, 
                                             tankBarrel.rotation.w);
    }

    private Quaternion ClampRotation(Quaternion q, Vector3 bounds)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, -bounds.x, bounds.x);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
        angleY = Mathf.Clamp(angleY, -bounds.y, bounds.y);
        q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

        float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);
        angleZ = Mathf.Clamp(angleZ, -bounds.z, bounds.z);
        q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);

        return q;
    }

    private float ClampBarrelX(Quaternion q, float clampAngle)
    {
        q.x /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, -clampAngle, clampAngle);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q.x;
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
