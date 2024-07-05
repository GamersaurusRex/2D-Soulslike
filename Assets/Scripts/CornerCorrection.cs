using UnityEngine;

public class CornerCorrection : MonoBehaviour
{
    public Transform LeftOuter;    
    public Transform RightOuter;
    public LayerMask hitLayer;
    public float cornerCheckDistance;
    public float verticalRayDistance;    
    public float horizontalRayDistance;

    private Vector3 LeftInnerPosition;
    private Vector3 RightInnerPosition;
    private Vector3 LeftOuterPosition;
    private Vector3 RightOuterPosition;
    private RaycastHit2D leftOuterHit;
    private RaycastHit2D leftInnerHit;
    private RaycastHit2D rightOuterHit;
    private RaycastHit2D rightInnerHit;
    private readonly float cornerOffset = 0.01f;
    private float playerDirection;

    void FixedUpdate()
    {
        LeftOuterPosition   = LeftOuter.position;
        RightOuterPosition  = RightOuter.position;

        if (transform.localScale.x > 0)
        {
            playerDirection = 1f;
        }
        else if(transform.localScale.x < 0)
        {
            playerDirection = -1f;
        }

        LeftInnerPosition = new(LeftOuter.position.x + playerDirection * cornerCheckDistance, LeftOuter.position.y);
        RightInnerPosition = new(RightOuter.position.x - playerDirection * cornerCheckDistance, RightOuter.position.y);

        leftOuterHit = Physics2D.Raycast(LeftOuterPosition, Vector2.up, verticalRayDistance, hitLayer);
        leftInnerHit = Physics2D.Raycast(LeftInnerPosition, Vector2.up, verticalRayDistance, hitLayer);
        rightOuterHit = Physics2D.Raycast(RightOuterPosition, Vector2.up, verticalRayDistance, hitLayer);
        rightInnerHit = Physics2D.Raycast(RightInnerPosition, Vector2.up, verticalRayDistance, hitLayer);

        if (leftOuterHit.collider != null && leftInnerHit.collider == null)             
        {            
            Vector2 horizontalRayStartPoint = new(LeftInnerPosition.x, LeftInnerPosition.y + verticalRayDistance);
            RaycastHit2D LeftHit = Physics2D.Raycast(horizontalRayStartPoint, playerDirection * Vector2.left, horizontalRayDistance, hitLayer);
            
            if(LeftHit.collider != null)
            {
                float distanceToMove = (LeftHit.point.x - leftOuterHit.point.x) * playerDirection + cornerOffset;
                transform.position = new Vector3(transform.position.x + playerDirection * distanceToMove, transform.position.y, transform.position.z);
            }            
        }
        else if(rightOuterHit.collider != null && rightInnerHit.collider == null)
        {
            Vector2 horizontalRayStartPoint = new(RightInnerPosition.x, RightInnerPosition.y + verticalRayDistance);
            RaycastHit2D RightHit = Physics2D.Raycast(horizontalRayStartPoint, playerDirection * Vector2.right, horizontalRayDistance, hitLayer);

            if (RightHit.collider != null)
            {
                float distanceToMove = (rightOuterHit.point.x - RightHit.point.x) * playerDirection + cornerOffset;
                transform.position = new Vector3(transform.position.x - playerDirection * distanceToMove, transform.position.y, transform.position.z);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(LeftOuterPosition, new(LeftOuterPosition.x, LeftOuterPosition.y + verticalRayDistance));
        Gizmos.DrawLine(LeftInnerPosition, new(LeftInnerPosition.x, LeftInnerPosition.y + verticalRayDistance));
        Gizmos.DrawLine(RightOuterPosition, new(RightOuterPosition.x, RightOuterPosition.y + verticalRayDistance));
        Gizmos.DrawLine(RightInnerPosition, new(RightInnerPosition.x, RightInnerPosition.y + verticalRayDistance));
    }
}
