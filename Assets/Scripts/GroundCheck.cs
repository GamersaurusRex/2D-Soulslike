using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Transform groundCheckObject;
    public Transform wallCheckObject;

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isWalled;

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckObject.position, 0.1f, groundLayer);        
        isWalled = Physics2D.OverlapCircle(wallCheckObject.position, 0.2f, wallLayer);
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsWalled()
    {
        return isWalled;
    }
}
