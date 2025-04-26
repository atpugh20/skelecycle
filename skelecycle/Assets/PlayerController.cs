using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    Rigidbody2D rb;
    Animator anim;

    InputAction moveAction;
    InputAction jumpAction;

    LayerMask GroundLayer;

    public InterfaceController interfaceController;

    private int jumpCount   = 0;
    private int jumpLimit   = 1;

    public float Speed      = 5.0f;
    public float JumpForce  = 12.0f;

    public float groundCheckDistance = 0.6f;

    public float AngleZ   = 0;
    public float SlipAngle = 70;

    private bool isDead = false;
    private bool isGrounded = false;

    public bool deathFinished = false;
    public bool WonGame = false;

    // Called once before first execution of Update 
    void Start() {
        Time.timeScale = 1f;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        
        GroundLayer = LayerMask.GetMask("GroundLayer");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
         RaycastHit2D hit = Physics2D.Raycast(
            transform.position, 
            Vector2.down,
            groundCheckDistance,
            GroundLayer
        );

        // Kill if running into wall or into hazard
        if (collision.gameObject.layer == LayerMask.NameToLayer("HazardLayer"))
            Kill();
        
        // Win game condition
        if (collision.gameObject.layer == LayerMask.NameToLayer("VictoryLayer"))
            WonGame = true;
    }

    // Update is called once per frame
    void Update() {

        // Check if paused before performing
        if (!interfaceController.GetIsPaused()) {
            CheckIfGrounded();
            CheckDeath();
            
            // Only peform player update if they are alive
            if (!isDead) {

                if (isGrounded)
                    jumpCount = 0;

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
            isGrounded = true;
        else
            isGrounded = false;
    }

    private void CheckDeath() {
        /**
         * If the angle is greater than 0.7 or less than -0.7
         */
        if ((rb.rotation >= SlipAngle ||
            rb.rotation <= -SlipAngle) &&
            isGrounded) {
            Kill();
        }
    }
    
    public void Kill() {
        isDead = true;
        anim.SetTrigger("Death");
        rb.linearVelocity = Vector2.zero;
    }

    public void FinishDeath() { anim.SetTrigger("DeathComplete"); }
}
