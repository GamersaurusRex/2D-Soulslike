using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerJump jump;
    private PlayerDodge dodge;

    void Start()
    {
        movement    = GetComponent<PlayerMovement>();
        jump        = GetComponent<PlayerJump>();
        dodge       = GetComponent<PlayerDodge>();
    }

    void Update()
    {
        HandlePlayerInput();              
    }

    void HandlePlayerInput()
    {
        movement.HandleMovement();
        jump.HandleJump();    
    }
}
