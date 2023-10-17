using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TankViewState
{
    ThirdPersonView,
    FirstPersonView
}

public class CameraRig : MonoBehaviour
{
    public static CameraRig Instance;

    [SerializeField] private Camera thirdPersonCam;
    [SerializeField] private Camera firstPersonCam;
    [SerializeField] private GameObject fpsViewShroud;
    [SerializeField] private float xSense;
    private TankViewState tankViewState = TankViewState.ThirdPersonView;

    [Space]

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

    Vector3 refVel;
    private bool zoom = false;
    private bool isSwitching = false;

    float camLookAngle = 0;
    float camPivotAngle = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }

    public Camera GetActiveCamera()
    {
        if(tankViewState == TankViewState.ThirdPersonView)
            return thirdPersonCam;
        else
            return firstPersonCam;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isSwitching)
        {
            StartCoroutine(SwitchCamera());
        }       

        ThirdPersonCameraControls();

        FirstPersonCameraControls();
    }

    private void ThirdPersonCameraControls()
    {
        if (tankViewState == TankViewState.FirstPersonView) return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            zoom = !zoom;
        }

        Zoom();

        FollowCam();

        RotateTankHeadThirdPerson();
        RotateThirdPersonCamera();
    }

    private void FirstPersonCameraControls()
    {
        if (tankViewState == TankViewState.ThirdPersonView) return;

        RotateTankHeadFirstPerson();
    }

    private IEnumerator SwitchCamera()
    {       
        if (tankViewState == TankViewState.FirstPersonView)
        {
            isSwitching = true;

            thirdPersonCam.gameObject.SetActive(true);
            firstPersonCam.gameObject.SetActive(false);

            fpsViewShroud.SetActive(false);
            tankViewState = TankViewState.ThirdPersonView;

            yield return new WaitForSeconds(0.2f);

            isSwitching = false;
            yield return null;
        }
        else if (tankViewState == TankViewState.ThirdPersonView)
        {
            isSwitching = true;

            thirdPersonCam.gameObject.SetActive(false);
            firstPersonCam.gameObject.SetActive(true);

            fpsViewShroud.SetActive(true);
            tankViewState = TankViewState.FirstPersonView;

            yield return new WaitForSeconds(0.2f);

            isSwitching = false;
            yield return null;
        }

    }

    private void Zoom()
    {
        if (zoom && thirdPersonCam.fieldOfView != 50)
        {
            thirdPersonCam.fieldOfView = Mathf.Lerp(thirdPersonCam.fieldOfView, 50, Time.deltaTime * zoomSpeed);
        }
        else if (!zoom && thirdPersonCam.fieldOfView != 75)
        {
            thirdPersonCam.fieldOfView = Mathf.Lerp(thirdPersonCam.fieldOfView, 75, Time.deltaTime * zoomSpeed);
        }       
    }

    private void FollowCam()
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, followTarget.position, ref cameraFollowVelocity, followSpeed);

        transform.position = targetPosition;
    }

    Vector3 lookAtPoint;
    private void RotateTankHeadThirdPerson()
    {
        if (Physics.Raycast(thirdPersonCam.transform.position + offset, thirdPersonCam.transform.forward + offset, out RaycastHit hitInfo, 10000f, layerMask))
        {
            lookAtPoint = hitInfo.point;
        }
        else
        {
            lookAtPoint = new Vector3(thirdPersonCam.transform.position.x + offset.x + thirdPersonCam.transform.forward.x * 100f,
                                      thirdPersonCam.transform.position.y + offset.y + thirdPersonCam.transform.forward.y * 100f,
                                      thirdPersonCam.transform.position.z + offset.z + thirdPersonCam.transform.forward.z * 100f);
        }

        Vector3 up = tankHead.up;
        Vector3 directionToTarget = Vector3.ProjectOnPlane(lookAtPoint - tankHead.position, up);
        Quaternion turretTargetDirection = Quaternion.LookRotation(directionToTarget, up);

        Quaternion from = Quaternion.LookRotation(tankHead.forward, tankHead.up);
        tankHead.rotation = Quaternion.RotateTowards(from, turretTargetDirection, headTurnSpeed * Time.fixedDeltaTime);

        tankBarrel.LookAt(lookAtPoint, Vector3.up);

        tankBarrel.rotation = new Quaternion(ClampBarrelX(tankBarrel.rotation, 13),
                                             tankHead.rotation.y,
                                             tankHead.rotation.z, 
                                             tankHead.rotation.w);

    }

    private void RotateTankHeadFirstPerson()
    {
        if (MouseInput().magnitude > 0)
        {
            Vector3 horizontalInput = new Vector3(0, Input.GetAxis("Mouse X"), 0);
            tankHead.Rotate(horizontalInput * Time.fixedDeltaTime * headTurnSpeed);

            Vector3 verticalInput = new Vector3(Input.GetAxis("Mouse Y"), 0, 0);
            tankBarrel.Rotate(-verticalInput);
            //tankBarrel.LookAt(-verticalInput, Vector3.up);
            //tankBarrel.rotation = new Quaternion(ClampBarrelX(tankBarrel.rotation, 13),
                                                 //tankHead.rotation.y,
                                                 //tankHead.rotation.z,
                                                 //tankBarrel.rotation.w);
        }
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

    private void RotateThirdPersonCamera()
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
