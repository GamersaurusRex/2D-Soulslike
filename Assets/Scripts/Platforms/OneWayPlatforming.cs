using UnityEngine;

public class OneWayPlatforming : MonoBehaviour
{
    private bool isPlayerOnPlatform;
    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        float verticalInput = InputManager.Instance.GetVerticalInput();
        bool jumpPressed    = InputManager.Instance.GetJumpInputDown();

        if (isPlayerOnPlatform && verticalInput < 0f && jumpPressed)
        {
            boxCollider.enabled = false;
            Invoke(nameof(EnableCollider), 0.5f);
        }
    }

    private void EnableCollider()
    {
        boxCollider.enabled = true;
    }

    private void SetPlayerOnPlatform(Collision2D other, bool value)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = value;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetPlayerOnPlatform(collision, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        SetPlayerOnPlatform(collision, false);
    }
}
