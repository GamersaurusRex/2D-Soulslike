using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public LayerMask groundLayer;
    public Transform groundCheckObject;

    [SerializeField] private bool isGrounded;

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckObject.position, 0.1f, groundLayer);
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}
