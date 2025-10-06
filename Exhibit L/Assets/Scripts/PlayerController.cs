using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]

    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float gravity = -30f;

    [Header("Camera References")]
    public Transform orientation;
    public Transform cameraPos;

    [Header("Character Controller")]
    [SerializeField] private CharacterController characterController;

    private bool grounded = true;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private PlayerMovement playerControls;
    private Vector3 currentMoveVelocity;
    private Vector3 velocity;

    private AreaScan areaScan;

    public float currentSpeed;
    public bool paused = false;

    private void Awake()
    {
        playerControls = new PlayerMovement();
        playerControls.Enable();
        characterController = GetComponent<CharacterController>();
        areaScan = GetComponent<AreaScan>();
    }


    // Update is called once per frame
    void Update()
    {
        grounded = characterController.isGrounded;
        MovePlayer();
        ApplyGravity();
        DampenHorizontalVelocityIfGrounded();

        if (!areaScan.toggle)
        {
            characterController.Move(currentMoveVelocity * Time.deltaTime);
            characterController.Move(velocity * Time.deltaTime);
        }

        currentSpeed = currentMoveVelocity.magnitude;
    }

    public void OnWASDInput(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
        verticalInput = context.ReadValue<Vector2>().y;
    }

    private void MovePlayer()
    {
        moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;

        currentMoveVelocity = Vector3.Lerp(currentMoveVelocity, moveDirection * moveSpeed, acceleration * Time.deltaTime);
    }

    private void DampenHorizontalVelocityIfGrounded()
    {
        if (grounded)
        {
            Vector3 horizontal = new Vector3(velocity.x, 0f, velocity.z);
            horizontal = Vector3.Lerp(horizontal, Vector3.zero, Time.deltaTime * 5f);
            velocity.x = horizontal.x;
            velocity.z = horizontal.z;
        }
    }

    //NOTE - split this function up into 2 seperate things later
    private void ApplyGravity()
    {
        if (!grounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0f)
        {
            velocity.y = -2f;
        }
    }

    public void Pause()
    {
        switch (paused)
        {
            case true:
                paused = false;
                GameManager.Instance.Pause(paused);
                playerControls.Enable();
                Time.timeScale = 1f;
                break;
            case false:
                paused = true;
                GameManager.Instance.Pause(paused);
                playerControls.Disable();
                Time.timeScale = 0f;
                break;
        }
            
        
    }
}
