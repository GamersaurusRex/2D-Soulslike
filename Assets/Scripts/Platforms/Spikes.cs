using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0f;
    [SerializeField] private float resetDelay = 1f;
    [SerializeField] private float maxVelocity = 10f;
    [SerializeField] private float verticalRayDistance = 100f;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private bool isResetable;

    private Rigidbody2D rb;
    private Vector3 originalPosition;
    private PolygonCollider2D spikeCollider;
    private bool isFallen = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        spikeCollider = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        if (rb.velocity.y < -maxVelocity)
        {
            rb.velocity = new Vector2(rb.velocity.x, -maxVelocity);
        }

        if(!isFallen)
        {
            RaycastHit2D playerHit = Physics2D.Raycast(transform.position, Vector2.down, verticalRayDistance, hitLayer);
            if (playerHit.collider != null)
            {
                isFallen = true;
                Invoke(nameof(Fall), fallDelay);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //reduce player health
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            if(isResetable)
            {
                Invoke(nameof(Reset), resetDelay);
            }
            else
            {                
                rb.bodyType = RigidbodyType2D.Kinematic;
                spikeCollider.enabled = false;
            }
            
        }

    }

    private void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Reset()
    {
        gameObject.SetActive(false);
        rb.bodyType = RigidbodyType2D.Kinematic;
        transform.position = originalPosition;
        isFallen = false;
        gameObject.SetActive(true);
    }
}
