using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    private PlayerMovement movement;
    private PlayerJump jump;
    private PlayerDash dash;
    private WallJump wallJump;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        jump     = GetComponent<PlayerJump>();
        dash     = GetComponent<PlayerDash>();
        wallJump = GetComponent<WallJump>();
    }

    void Update()
    {
        HandlePlayerInput();              
    }

    void HandlePlayerInput()
    {
        movement.HandleMovement();
        jump.HandleJump();
        dash.HandleDash();
        wallJump.HandleWallJump();
    }
}
