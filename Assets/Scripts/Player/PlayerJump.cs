using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [Tooltip("Max Jump Height between 1 and 10")]
    public float maxJumpHeight;
    [Tooltip("Time to reach max jump height between 0.1 and 2.0")]
    public float timeToMaxHeight;

    [Header("Gravity Settings")]
    [Tooltip("Gravity applied on player after reaching max jump height")]
    public float downGravityModifier;           // used to fall down faster
    [Tooltip("Gravity applied on player before reaching max jump height")]
    public float upGravityModifier;             // used to rise to the height slower
    [Tooltip("Gravity applied on player at the max jump height")]
    public float peakGravityModifier;           // used to get extra hang time at peak
    [Tooltip("Velocity to check for peak gravity modifer")]
    public float velocityAtPeak;                // used to set the peak gravity from -this veolicty to +this velocity
    [Tooltip("Gravity applied on player when jump button is released")]
    public float jumpCutOff;

    [Header("Jump Feel Settings")]
    [Tooltip("Buffer time to allow player to jump again even before landing")]
    public float jumpBuffer;
    [Tooltip("Buffer time to allow player to jump after falling from a platform")]
    public float coyoteTime;

    private bool isGrounded;
    private bool jumpRequest;
    private bool jumpPressed;
    private bool currentlyJumping;

    private float jumpSpeed;
    private float jumpBufferCounter;
    private float gravityMultiplier;
    private float coyoteTimeCounter;
    private float groundGravity;
    private readonly float defaultGravityScale = 1f;

    private Vector2 velocity;
    private Rigidbody2D rb;
    private GroundCheck groundCheck;
    private PlayerDash dash;
    private WallJump wallJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<GroundCheck>();
        dash = GetComponent<PlayerDash>();
        wallJump = GetComponent<WallJump>();
        groundGravity = -2f * maxJumpHeight / (timeToMaxHeight * timeToMaxHeight);
    }

    public void HandleJump()
    {        
        HandleInput();
        GetGroundCheck();        
        CacheJump();
        CoyoteTime();
    }

    private void FixedUpdate()
    {
        SetGravityScale();

        velocity = rb.velocity;

        if(jumpRequest)
        {
            DoAJump();
            rb.velocity = velocity;
            return;
        }

        CalculateGravity();
        rb.velocity = velocity;
    }

    private void GetGroundCheck()
    {
        isGrounded = groundCheck.IsGrounded();
    }

    private void HandleInput()
    {
        if (InputManager.Instance.GetJumpInputDown() && !dash.isDashing && InputManager.Instance.GetVerticalInput() >= 0f)
        {
            jumpRequest = true;
            jumpPressed = true;
        }

        if (InputManager.Instance.GetJumpInputUp() && !dash.isDashing)
        {
            jumpPressed = false;
        }
    }

    private void SetGravityScale()
    {
        // If player is dashing, deactivate gravity
        if (dash.isDashing)
        {
            gravityMultiplier = 0f;
            return;
        }

        rb.gravityScale = (groundGravity / Physics2D.gravity.y) * gravityMultiplier;
    }

    private void CacheJump()
    {
        //Jump buffer allows us to queue up a jump, which will play when we next hit the ground
        if (jumpBuffer > 0 && jumpRequest)
        {
            //Instead of immediately turning off "jumpRequest", start counting up
            //All the while, the DoAJump function will repeatedly be fired off
            jumpBufferCounter += Time.deltaTime;

            if (jumpBufferCounter > jumpBuffer)
            {
                //If time exceeds the jump buffer, turn off "jumpRequest"
                jumpRequest = false;
                jumpBufferCounter = 0;
            }
        }
    }

    private void CoyoteTime()
    {
        //If we're not on the ground and we're not currently jumping, that means we've stepped off the edge of a platform. So, start the coyote time counter
        if (!currentlyJumping && !isGrounded)
        {
            coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            //Reset it when we touch the ground, or jump
            coyoteTimeCounter = 0;
        }
    }

    private void DoAJump()
    {
        //Create the jump, provided we are on the ground, in coyote time
        if (isGrounded || (coyoteTimeCounter > 0.03f && coyoteTimeCounter < coyoteTime))
        {
            jumpRequest = false;
            currentlyJumping = true;
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            //Determine the power of the jump
            // v = /2gh
            // negative is placed because gravity is already negative. so, inside sqrt will be positive
            jumpSpeed = Mathf.Sqrt(-2f * groundGravity * maxJumpHeight);

            //If player is moving up or down when he jumps, change the jumpSpeed
            //This will ensure the jump is the exact same strength, no matter your velocity.
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(rb.velocity.y);
            }

            //Apply the new jumpSpeed to the velocity. It will be sent to the Rigidbody in FixedUpdate;
            velocity.y += jumpSpeed;            
        }

        if (jumpBuffer == 0)
        {
            //If we don't have a jump buffer, then turn off desiredJump immediately after hitting jumping
            jumpRequest = false;
        }
    }

    private void CalculateGravity()
    {
        if(dash.isDashing)
        {
            gravityMultiplier = defaultGravityScale;
            return;
        }

        if (wallJump.isWallSliding)
        {
            gravityMultiplier = downGravityModifier;
            return;
        }

        if (wallJump.isWallJumping)
        {
            currentlyJumping = false;
            gravityMultiplier = downGravityModifier;
            return;
        }

        if(wallJump.justWallJumped && rb.velocity.y > velocityAtPeak)
        {
            gravityMultiplier = downGravityModifier;
            return;
        }

        // If player on ground, set gravity to default
        if (isGrounded)
        {
            gravityMultiplier = defaultGravityScale;
            currentlyJumping = false;
            return;
        }

        //If player is going up--------------------------------------------------
        if (rb.velocity.y > velocityAtPeak && currentlyJumping)
        {            
            //Apply upward multiplier if player is rising and holding jump
            if (jumpPressed)
            {
                gravityMultiplier = upGravityModifier;
            }
            //But apply a special downward multiplier if the player lets go of jump
            else
            {
                gravityMultiplier = jumpCutOff;
            }            
        }

        //Else if going down------------------------------------------------------
        else if (rb.velocity.y < -velocityAtPeak)
        {
            //Apply the downward gravity multiplier as player comes back to ground
            gravityMultiplier = downGravityModifier;
        }

        //Else not moving vertically at all----------------------------------------
        else 
        {
            if(currentlyJumping)
            {
                gravityMultiplier = peakGravityModifier;
            }
            else
            {
                gravityMultiplier = downGravityModifier; 
            }
        }
    }
}
