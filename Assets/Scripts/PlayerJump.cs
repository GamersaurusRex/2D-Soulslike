using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float downGravityModifier;
    public float upGravityModifier;
    public float maxJumpHeight;
    public float timeToMaxHeight;
    public float jumpCutOff;
    public float jumpBuffer;
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<GroundCheck>();
        groundGravity = -2f * maxJumpHeight / (timeToMaxHeight * timeToMaxHeight);
    }

    public void HandleJump()
    {                
        HandleInput();
        DoGroundCheck();        
        CacheJump();
        CoyoteTime();
    }

    private void FixedUpdate()
    {
        SetGravityScale();

        velocity = rb.velocity;

        if(jumpRequest && isGrounded)
        {
            DoAJump();
            rb.velocity = velocity;
            return;
        }

        CalculateGravity();
        rb.velocity = velocity;
    }

    private void DoGroundCheck()
    {
        isGrounded = groundCheck.IsGrounded();
        if (isGrounded)
        {
            currentlyJumping = false;
        }
    }

    private void HandleInput()
    {
        if (InputManager.Instance.GetJumpInputDown())
        {
            jumpRequest = true;
            jumpPressed = true;
        }

        if (InputManager.Instance.GetJumpInputUp())
        {
            jumpPressed = false;
        }
    }

    private void SetGravityScale()
    {
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
            currentlyJumping = true;
        }

        if (jumpBuffer == 0)
        {
            //If we don't have a jump buffer, then turn off desiredJump immediately after hitting jumping
            jumpRequest = false;
        }
    }

    private void CalculateGravity()
    {
        // If player on ground, set gravity to default
        if (isGrounded)
        {
            gravityMultiplier = defaultGravityScale;
        }

        //If player is going up--------------------------------------------------
        if (rb.velocity.y > 0.01f && currentlyJumping)
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
        else if (rb.velocity.y < -0.01f)
        {
            //Apply the downward gravity multiplier as player comes back to ground
            gravityMultiplier = downGravityModifier;
        }

        //Else not moving vertically at all----------------------------------------
        else
        {
            gravityMultiplier = defaultGravityScale;
        }
    }
}
