using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    private float moveInput;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void HandleMovement()
    {
        moveInput   = InputManager.Instance.GetHorizontalInput();
        float targetSpeed = moveSpeed * moveInput;
        rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
    }
}
