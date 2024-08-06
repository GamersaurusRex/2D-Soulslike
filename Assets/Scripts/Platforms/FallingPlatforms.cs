using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1f;
    [SerializeField] private float fallingTime = 1f;
    [SerializeField] private float resetDelay = 1f;
    [SerializeField] private float maxVelocity = 10f;

    private Rigidbody2D rb;
    private Vector3 originalPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
    }

    private void Update()
    {
        if(rb.velocity.y < -maxVelocity)
        {
            rb.velocity = new Vector2(rb.velocity.x, -maxVelocity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Invoke(nameof(Fall),fallDelay);
        }
    }

    private void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        Invoke(nameof(Reset), fallingTime);
    }

    private void Reset()
    {
        gameObject.SetActive(false);
        rb.bodyType = RigidbodyType2D.Kinematic;
        transform.position = originalPosition;
        Invoke(nameof(Activate), resetDelay);
    }

    private void Activate()
    {
        gameObject.SetActive(true);
    }
}
