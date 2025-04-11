using System;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float horizontalSpeed = 2.0F;
    [SerializeField]
    private float verticalSpeed = 2.0F;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Transform cameraParent;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        //hide mouse
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Update()
    {
        
        HandlePlayerRotation();
        HandlePlayerMovement();
        
        // unlock mouse when escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    
    private void HandlePlayerMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        
        //clamp the movement vector to a maximum length of 1
        if (movement.sqrMagnitude > 1)
        {
            movement.Normalize();
        }
        
        // Rotate the movement vector to match the camera's orientation
        movement = cameraParent.TransformDirection(movement);
        
        _rb.linearVelocity = movement * speed;
    }
    
    private void HandlePlayerRotation()
    {
        float h = horizontalSpeed * Input.GetAxis("Mouse X");
        float v = verticalSpeed * Input.GetAxis("Mouse Y");
        cameraParent.Rotate(0, h, 0);
        cameraTransform.Rotate(-v, 0, 0);
    }
}
