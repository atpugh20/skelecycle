using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    Rigidbody2D rb;
    Animator anim;

    InputAction moveAction;
    InputAction jumpAction;

    private LayerMask GroundLayer;

    private int jumpCount   = 0;
    private int jumpLimit   = 1;

    public float Speed      = 5.0f;
    public float JumpForce  = 12.0f;

    public float groundCheckDistance = 0.6f;

    public float AngleZ   = 0;
    public float AngleAcc = 1;

    // Called once before first execution of Update 
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        
        GroundLayer = LayerMask.GetMask("GroundLayer");
    }

    // Update is called once per frame
    void Update() {
        CheckIfGrounded(); 
        
        // Movement
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        rb.AddForce(new Vector2(1, 0) * Speed);
        if (rb.linearVelocityX > Speed) {
            rb.linearVelocityX = Speed;
        }
        
        // Set the angle that the player will rotate at based on the speed the player is moving
        if (AngleZ < 0 && moveValue.x == 0)
            AngleZ -= rb.linearVelocityX * 0.2f;
        else if (moveValue.x == 0)
            AngleZ += rb.linearVelocityX * 0.2f;
        else
            AngleZ -= moveValue.x;
        
        // Set rotation and animation value
        rb.MoveRotation(AngleZ);
        anim.SetFloat("Rotation", rb.transform.rotation.z);

        // Jumping
        if (jumpCount < jumpLimit) {
            if (jumpAction.WasPerformedThisFrame()) {
                jumpCount++;
                rb.linearVelocityY = JumpForce;
            }
        }
    }

    private void CheckIfGrounded() {
        /** 
         * Uses a raycast from the middle of the player to
         * detect if it has collided with the ground.
         */        
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, 
            Vector2.down,
            groundCheckDistance,
            GroundLayer
        );
        
        // Limits the user to jumpCount jumps
        if (hit.collider != null && rb.linearVelocityY <= 0) 
            jumpCount = 0;
    }

    private void CheckForSlip() {
        /**
         * If the angle is greater than 0.7 or less than -0.7
         */
         
    }
}
