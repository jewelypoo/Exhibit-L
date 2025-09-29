using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]

    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float gravity = -30f;
    [SerializeField] private float airControlMultiplier = 0.2f;

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
    private bool hasPogoed = false;
    private Vector3 currentMoveVelocity;
    private Vector3 velocity;
    private bool canPogo = true;

    public float currentSpeed;

    private void Awake()
    {
        playerControls = new PlayerMovement();
        playerControls.Enable();
        characterController = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        grounded = characterController.isGrounded;
        MovePlayer();
        ApplyGravityAndJump();
        DampenHorizontalVelocityIfGrounded();

        characterController.Move(currentMoveVelocity * Time.deltaTime);
        characterController.Move(velocity * Time.deltaTime);

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

        float control = (!grounded && hasPogoed) ? airControlMultiplier : 1f;

        currentMoveVelocity = Vector3.Lerp(currentMoveVelocity, moveDirection * moveSpeed * control, acceleration * Time.deltaTime);

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
    private void ApplyGravityAndJump()
    {
        if (!grounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0f)
        {
            hasPogoed = false;
            velocity.y = -2f;
        }
    }


  

 
}
