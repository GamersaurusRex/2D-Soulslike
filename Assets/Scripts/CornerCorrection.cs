using UnityEngine;

public class CornerCorrection : MonoBehaviour
{
    public Transform TopLeft;    
    public Transform TopRight;
    public Transform BottomRight;
    public LayerMask hitLayer;
    public float cornerJumpCheckDistance;
    public float cornerDashCheckDistance;
    public float verticalRayDistance;    
    public float horizontalRayDistance;
    public float cornerJumpOffset;
    public float cornerDashOffset;

    private Vector3 LeftInnerPosition;
    private Vector3 RightInnerPosition;
    private Vector3 LeftOuterPosition;
    private Vector3 RightOuterPosition;
    private RaycastHit2D leftOuterHit;
    private RaycastHit2D leftInnerHit;
    private RaycastHit2D rightOuterHit;
    private RaycastHit2D rightInnerHit;

    private Vector3 TopInnerPosition;
    private Vector3 BottomInnerPosition;
    private Vector3 TopOuterPosition;
    private Vector3 BottomOuterPosition;
    private RaycastHit2D topOuterHit;
    private RaycastHit2D topInnerHit;
    private RaycastHit2D bottomOuterHit;
    private RaycastHit2D bottomInnerHit;

    private float playerDirection;
    private PlayerDash dash;
    private Rigidbody2D rb;

    private void Start()
    {
        dash = GetComponent<PlayerDash>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        GetPlayerDirection();

        if(dash.isDashing)
        {
            DashCorrection();
        }

        if(rb.velocity.y > 0)
        {
            JumpCorrection();
        }        
    }

    private void GetPlayerDirection()
    {
        if (transform.localScale.x > 0)
        {
            playerDirection = 1f;
        }
        else if (transform.localScale.x < 0)
        {
            playerDirection = -1f;
        }
    }

    private void JumpCorrection()
    {
        LeftOuterPosition = TopLeft.position;
        RightOuterPosition = TopRight.position;

        LeftInnerPosition = new(TopLeft.position.x + playerDirection * cornerJumpCheckDistance, TopLeft.position.y);
        RightInnerPosition = new(TopRight.position.x - playerDirection * cornerJumpCheckDistance, TopRight.position.y);

        leftOuterHit = Physics2D.Raycast(LeftOuterPosition, Vector2.up, verticalRayDistance, hitLayer);
        leftInnerHit = Physics2D.Raycast(LeftInnerPosition, Vector2.up, verticalRayDistance, hitLayer);
        rightOuterHit = Physics2D.Raycast(RightOuterPosition, Vector2.up, verticalRayDistance, hitLayer);
        rightInnerHit = Physics2D.Raycast(RightInnerPosition, Vector2.up, verticalRayDistance, hitLayer);

        if (leftOuterHit.collider != null && leftInnerHit.collider == null)
        {
            Vector2 horizontalRayStartPoint = new(LeftInnerPosition.x, LeftInnerPosition.y + verticalRayDistance);
            RaycastHit2D LeftHit = Physics2D.Raycast(horizontalRayStartPoint, playerDirection * Vector2.left, horizontalRayDistance, hitLayer);

            if (LeftHit.collider != null)
            {
                float distanceToMove = (LeftHit.point.x - leftOuterHit.point.x) * playerDirection + cornerJumpOffset;
                transform.position = new Vector3(transform.position.x + playerDirection * distanceToMove, transform.position.y, transform.position.z);
            }
        }
        else if (rightOuterHit.collider != null && rightInnerHit.collider == null)
        {
            Vector2 horizontalRayStartPoint = new(RightInnerPosition.x, RightInnerPosition.y + verticalRayDistance);
            RaycastHit2D RightHit = Physics2D.Raycast(horizontalRayStartPoint, playerDirection * Vector2.right, horizontalRayDistance, hitLayer);

            if (RightHit.collider != null)
            {
                float distanceToMove = (rightOuterHit.point.x - RightHit.point.x) * playerDirection + cornerJumpOffset;
                transform.position = new Vector3(transform.position.x - playerDirection * distanceToMove, transform.position.y, transform.position.z);
            }
        }
    }

    private void DashCorrection()
    {
        TopOuterPosition = TopRight.position; 
        BottomOuterPosition = BottomRight.position;

        TopInnerPosition = new(TopRight.position.x, TopRight.position.y - cornerDashCheckDistance);
        BottomInnerPosition = new(BottomRight.position.x, BottomRight.position.y + cornerDashCheckDistance);

        topOuterHit = Physics2D.Raycast(TopOuterPosition, Vector2.right * playerDirection, verticalRayDistance, hitLayer);
        topInnerHit = Physics2D.Raycast(TopInnerPosition, Vector2.right * playerDirection, verticalRayDistance, hitLayer);
        bottomOuterHit = Physics2D.Raycast(BottomOuterPosition, Vector2.right * playerDirection, verticalRayDistance, hitLayer);
        bottomInnerHit = Physics2D.Raycast(BottomInnerPosition, Vector2.right * playerDirection, verticalRayDistance, hitLayer);

        if (topOuterHit.collider != null && topInnerHit.collider == null)
        {
            Vector2 verticalRayStartPoint = new(TopInnerPosition.x + verticalRayDistance, TopInnerPosition.y);
            RaycastHit2D TopHit = Physics2D.Raycast(verticalRayStartPoint, Vector2.up, horizontalRayDistance, hitLayer);

            if (TopHit.collider != null)
            {
                float distanceToMove = (topOuterHit.point.y - TopHit.point.y) + cornerDashOffset;
                transform.position = new Vector3(transform.position.x, transform.position.y - distanceToMove, transform.position.z);
            }
        }
        else if (bottomOuterHit.collider != null && bottomInnerHit.collider == null)
        {
            Vector2 verticalRayStartPoint = new(BottomInnerPosition.x + verticalRayDistance, BottomInnerPosition.y);
            RaycastHit2D BottomHit = Physics2D.Raycast(verticalRayStartPoint, Vector2.down, horizontalRayDistance, hitLayer);

            if (BottomHit.collider != null)
            {
                float distanceToMove = (BottomHit.point.y - bottomOuterHit.point.y) + cornerDashOffset;
                transform.position = new Vector3(transform.position.x, transform.position.y + distanceToMove, transform.position.z);
            }
        }
    }
}
