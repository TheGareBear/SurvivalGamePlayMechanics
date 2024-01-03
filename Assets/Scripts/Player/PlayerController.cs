using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private UnityEngine.Vector2 curMovementInput;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private UnityEngine.Vector2 mouseDelta;

    private Rigidbody rig;

    void Awake ()
    {
        // get our components
        rig = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move();
    }

    void LateUpdate() 
    {
        CameraLook();
    }

    void Move()
    {
        UnityEngine.Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rig.velocity.y;
        rig.velocity = dir;
    }

    void CameraLook() 
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new UnityEngine.Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new UnityEngine.Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnLookInput(InputAction.CallbackContext context) 
    {
        mouseDelta = context.ReadValue<UnityEngine.Vector2>();
    }

        public void OnMoveInput(InputAction.CallbackContext context) 
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<UnityEngine.Vector2>();
        } 
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = UnityEngine.Vector2.zero;
        }
    }
}
