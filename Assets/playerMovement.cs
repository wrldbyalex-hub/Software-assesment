// Programmer: Alex
// Project: Software engineering assesment 
// this is the script for the player movement

using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed = 5f; // sets speed 
    public float jumpForce = 10f; // sets jump force

    public Rigidbody2D rb; // creates a variable for the rigidbody
    private float moveInput; // creates a variable for the movement input
    private bool isGrounded; // creates a variable to check if the player is on the ground
    
    private SpriteRenderer spriteRenderer; // this is for animations and making it so there isn't tp when they change direction
    

    public Transform groundCheck; // this is for the ground check
    public float groundCheckRadius; // this is for the ground check
    public LayerMask normalGround; // this is for the ground check, it checks if the player is on the ground by checking if there is a collider in the ground check radius that is on the normal ground layer

    private Animator animator; // this is for animations (again)
    
    public int maxHealth = 3; // Heatlh system, sets the max health to 3
    public int currentHealth; // Current Heatlh (for when the player takes damage)
    void Start() // what happens when the game starts
    {
        animator = GetComponentInChildren<Animator>(); // this is for animations, it gets the animator component from the game object
        rb = GetComponent<Rigidbody2D>(); // this is for the rigidbody, it gets the rigidbody component from the game object
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // this is for the sprite renderer, it gets the sprite renderer component from the game object
        currentHealth = maxHealth; // this sets the current health to the max health at the start of the game
    }
    
    void Update() // what happens every frame
    {
        // this is the movement
        moveInput = Input.GetAxis("Horizontal");

        // this is the jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // speed of the jump 
        }
        if(moveInput != 0) // for the animation to stop telaportation
        {
            animator.SetBool("isWalking", true); // this and the if else statements make the character face the right way
            
            if (moveInput < 0)
                spriteRenderer.flipX = true;
            if (moveInput > 0)
                spriteRenderer.flipX = false;
        }
        else
        {
            animator.SetBool("isWalking", false); // will cause the idle animaiton to play 
        }
    }

    void FixedUpdate() // this is for the physics, it runs at a fixed time interval
    {
        // this is the movement
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y); // this is the jump, it sets the velocity of the rigidbody to the jump force when the player jumps

        // this is the ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, normalGround); // this checks if there is a collider in the ground check radius that is on the normal ground layer, if there is, it sets isGrounded to true, if there isn't, it sets isGrounded to false
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // when player takes damage it is subtracted from the current health
        Debug.Log("WARNING: Security breach! System health at " + currentHealth); // prints current health to console 

        // For death
        if (currentHealth <= 0)
        {
            currentHealth = 0; // In case enimies do more damage than the player's current HP
            Debug.Log("CRITICAL ERROR: System breached! Game Over."); // prints the death thing to the console. 
        }
    }
}

