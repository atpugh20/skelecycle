using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    Rigidbody2D rb;
    InputAction moveAction;
    InputAction jumpAction;

    private bool isGrounded;

    private int jumpCount   = 0;
    private int jumpLimit   = 2;

    public float Speed      = 2.5f;
    public float JumpForce  = 5.0f;

    public float groundCheckDistance = 0.001f;
    
    void Start() {
        /**
         * Called once before the first execution of Update after the MonoBehaviour is created
         */
        rb = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update() {
        /** 
         * Update is called once per frame
         */

        // Collision
        isGrounded = Physics2D.Raycast(
            transform.position, 
            Vector2.down,
            groundCheckDistance
        );

        if (isGrounded)
            jumpCount = 0;

        Debug.DrawRay(transform.position, Vector2.down, Color.green);

        // Movement
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        rb.linearVelocityX = moveValue.x * Speed;


        // Jumping
        if (jumpCount < jumpLimit) {
            if (jumpAction.WasPerformedThisFrame()) {
                jumpCount++;
                rb.linearVelocityY = JumpForce;
            }
        }
    }
}
