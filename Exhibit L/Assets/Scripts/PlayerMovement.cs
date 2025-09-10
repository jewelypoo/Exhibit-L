using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Sharkey, Logan
/// Created: 9/9/2025
/// This script handles all of the players movement, from ground to pogo
/// </summary>


public class PlayerMovementHandler : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float groundDrag;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private bool grounded = false;
    public LayerMask whatIsGround;

    [Header("Misc")]
    public Transform orientation;

    private PlayerMovement playerControls;
    private Rigidbody rb;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;



    public void Awake()
    {
        playerControls = new PlayerMovement();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerControls.Enable();
    }

    private void Update()
    {
        //Check if on ground             0.5f is half default capsule height(change if player model ever changes) and 0.2f is extra check length past the bottom
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        PlayerInput();
        SpeedControl();

        if (grounded)
            rb.linearDamping = groundDrag;
        else
        {
            rb.linearDamping = 0;
        }

        MovePlayer();
    }


    private void PlayerInput()
    {
        horizontalInput = playerControls.PlayerMove.WASDInput.ReadValue<Vector2>().x;
        verticalInput = playerControls.PlayerMove.WASDInput.ReadValue<Vector2>().y;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //Checks if player velocity is higher than it should be
        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVelocity = flatVel.normalized * speed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }
}
