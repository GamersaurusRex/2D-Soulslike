using UnityEngine;

public class WallJump : MonoBehaviour
{
    [Header("Wall Jump Settings")]
    [Tooltip("Speed with which the player slides down along a wall")]
    public float WallSlideSpeed;
    [Tooltip("Time to turn away from wall and still be able to jump")]
    public float wallJumpBuffer;
    [Tooltip("Duration between each successive wall jump to keep the player in wall jumping state")]
    public float wallJumpDuration;
    [Tooltip("Jump speed in X and Y direction from a wall")]
    public Vector2 wallJumpPower;

    public bool isWallJumping;
    public bool justWallJumped;
    public bool isWallSliding;

    private Rigidbody2D rb;
    private GroundCheck groundCheck;

    private float moveInput;
    private float wallJumpDirection;
    private float wallJumpTimer;    

    private bool jumpRequest;
    private bool isWalled;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<GroundCheck>();
    }

    public void HandleWallJump()
    {           
        GetCheckValues();
        HandleInput();
    }

    private void HandleInput()
    {
        moveInput = InputManager.Instance.GetHorizontalInput();

        if (InputManager.Instance.GetJumpInputDown() && wallJumpTimer > 0f)
        {
            jumpRequest = true;
        }
    }

    private void FixedUpdate()
    {
        WallSlide();
        WallJumping();
    }

    private void GetCheckValues()
    {
        isGrounded = groundCheck.IsGrounded();
        isWalled = groundCheck.IsWalled();
    }

    private void WallSlide()
    {        
        //if (isWalled && !isGrounded && moveInput != 0f)
        if (isWalled && !isGrounded)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -WallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJumping()
    {
        if(isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpBuffer;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpTimer -= Time.deltaTime;
        }

        if(jumpRequest && wallJumpTimer > 0f)
        {
            
            jumpRequest = false;
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0f;

            if(transform.localScale.x != wallJumpDirection)
            {
                transform.localScale = new Vector3(wallJumpDirection, transform.localScale.y, transform.localScale.z);   
            }

            Invoke(nameof(StopWallJumping), wallJumpDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
        justWallJumped = true;
        Invoke(nameof(EndJustWallJumped), wallJumpDuration);
    }

    private void EndJustWallJumped()
    {
        justWallJumped = false;
    }
}
