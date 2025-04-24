using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    Rigidbody2D rb;
    InputAction moveAction;
    InputAction jumpAction;
    private LayerMask GroundLayer;

    private int jumpCount   = 0;
    private int jumpLimit   = 1;

    public float Speed      = 5.0f;
    public float JumpForce  = 10.0f;

    public float groundCheckDistance = 0.6f;
    
    void Start() {
        /**
         * Called once before the first execution of Update after the MonoBehaviour is created
         */
        rb = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        GroundLayer = LayerMask.GetMask("GroundLayer");
    }

    void Update() {
        /** 
         * Update is called once per frame
         */
        checkIfGrounded(); 
        
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

    private void checkIfGrounded() {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, 
            Vector2.down,
            groundCheckDistance,
            GroundLayer
        );
        
        if (hit.collider != null && rb.linearVelocityY <= 0) 
            jumpCount = 0;

        Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, Color.red);
    }
}
