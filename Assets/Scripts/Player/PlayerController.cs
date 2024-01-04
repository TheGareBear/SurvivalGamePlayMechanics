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
    public float jumpForce;
    public LayerMask groundLayerMask;

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

    public void OnJumpInput (InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            if(IsGrounded())
            {
                rig.AddForce(UnityEngine.Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void OnDrawGizmos ()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position + (transform.forward * 0.2f) + (UnityEngine.Vector3.up * 0.01f), UnityEngine.Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f) + (UnityEngine.Vector3.up * 0.01f), UnityEngine.Vector3.down);
        Gizmos.DrawRay(transform.position + (transform.right * 0.2f) + (UnityEngine.Vector3.up * 0.01f), UnityEngine.Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.right * 0.2f) + (UnityEngine.Vector3.up * 0.01f), UnityEngine.Vector3.down);
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f), UnityEngine.Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f), UnityEngine.Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f), UnityEngine.Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f), UnityEngine.Vector3.down)
        };

        for(int i = 0; i < rays.Length; i++)
        {
            if(Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        
        return false;
    }
}
