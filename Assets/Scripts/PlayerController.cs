using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Vivox;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [Header("Base setup")]
    [SerializeField] private float walkingSpeed = 7.5f;
    [SerializeField] private float runningSpeed = 11.5f;
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 20.0f;
    [SerializeField] private float lookSpeed = 2.0f;
    [SerializeField] private float lookXLimit = 45.0f;
    [SerializeField] private float cameraYOffset = 0.4f;


    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    bool isRunning = false;
    bool isMovementDisabled = false;

    private float cameraVerticalRotation = 0;
    private Camera playerCamera;
    private bool clientInitialized = false;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsOwner)
        {
            playerCamera = Camera.main;
            playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y + cameraYOffset, transform.position.z);
            playerCamera.transform.SetParent(transform);
        }
        else
        {
            gameObject.GetComponent<PlayerController>().enabled = false;
        }
        clientInitialized = true;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnEnable()
    {
        VoiceController.instance.OnVivoxReady += StartUpdatingVoicePosition;
    }

    private void OnDisable()
    {
        VoiceController.instance.OnVivoxReady -= StartUpdatingVoicePosition;
    }
    void Update()
    {
        if (!clientInitialized || !IsOwner)
            return;


        if (!isMovementDisabled)
        {
            GetMovementInput();
            GetCameraInput();
        }

        // Move player
        ApplyGravity();
        characterController.Move(moveDirection * Time.deltaTime);

        // Menu input
        GetMenuInput();
    }

    private void StartUpdatingVoicePosition()
    {
        VivoxService.Instance.Set3DPosition(
            this.gameObject,
            "GameWorld"
        );
    }

    private void GetMovementInput()
    {
        // Horizontal plane movement (X-Z axis)
        isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector3 rawDirection = (transform.right * Input.GetAxis("Horizontal")) +
                               (transform.forward * Input.GetAxis("Vertical"));
    
        if (rawDirection.magnitude > 1f)
        {
            rawDirection.Normalize();
        }

        float currMoveDirectionY = moveDirection.y; // Preserve the current vertical movement
        moveDirection = rawDirection * (isRunning ? runningSpeed : walkingSpeed);
        moveDirection.y = currMoveDirectionY;

        // Vertical movement (Y axis)
        if (Input.GetButton("Jump") && characterController.isGrounded)
        {
            moveDirection.y = jumpForce;
        }

        
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
    }

    private void GetCameraInput()
    {
        // Horizontal rotation
        float mouseX = Input.GetAxis("Mouse X");
        Quaternion horizontalRotation = Quaternion.Euler(0f, mouseX * lookSpeed, 0f);
        transform.rotation *= horizontalRotation; // Rotate player around Y-axis

        // Vertical rotation
        float mouseY = -Input.GetAxis("Mouse Y"); // Invert Y-axis for camera movement
        cameraVerticalRotation += mouseY * lookSpeed;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f); // Rotate camera vertically
    }

    private void GetMenuInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isMenuOpen = GameMenuManager.ToggleGameMenu();

            if (!isMenuOpen)
            {
                // Lock cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                // Unlock cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            isMovementDisabled = isMenuOpen;
        }
    }

}