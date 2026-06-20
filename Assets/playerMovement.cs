// Programmer: Alex
// this is the script for the player movement

using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed = 5f; // sets speed 
    public float jumpForce = 10f; // sets jump force

    private Coroutine activeFeedbackRoutine; // to keep track of the active feedback coroutine
    [Header("Damage Feedback")]
    public SpriteRenderer playerSprite;
    public UnityEngine.UI.Image screenFlash;
    public float invulnerabilityDuration = 1.5f; // how long the player is invulnerable
    public float flashDuration = 0.1f; // how long the flash lasts
    public bool isInvulnerable = false; // if the player is invulnerable or not

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
    public HealthUIManager healthUI; // Reference to the hearts UI so we can update it when health changes

    void Start() // what happens when the game starts
    {
        animator = GetComponentInChildren<Animator>(); // this is for animations, it gets the animator component from the game object
        rb = GetComponent<Rigidbody2D>(); // this is for the rigidbody, it gets the rigidbody component from the game object
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // this is for the sprite renderer, it gets the sprite renderer component from the game object
        currentHealth = maxHealth; // this sets the current health to the max health at the start of the game

        // Builds the starting row of hearts to match maxHealth
        if (healthUI != null)
        {
            healthUI.InitializeHearts(maxHealth);
        }
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
        // Ignore damage if the player is currently invulnerable
        if (isInvulnerable) return;

        currentHealth -= damage; 
        Debug.Log("WARNING: Security breach! System health at " + currentHealth); 

        // Stops any other loops before starting a new one
        if (activeFeedbackRoutine != null)
        {
            StopCoroutine(activeFeedbackRoutine);
        }
    
        // starting a new loop
        activeFeedbackRoutine = StartCoroutine(UnifiedFeedbackLoop());

        if (currentHealth <= 0)
        {
            currentHealth = 0; 
            Debug.Log("CRITICAL ERROR: System breached! Game Over."); 
        }

        // Updates the hearts UI to reflect the new (clamped) health value
        if (healthUI != null)
        {
            healthUI.UpdateHearts(currentHealth);
        }
    }

    private System.Collections.IEnumerator UnifiedFeedbackLoop()
    {
        isInvulnerable = true;

        // triggering the full screen flash

        if (screenFlash != null)
        {
            Color flashColor = screenFlash.color;
            flashColor.a = 0.2f; // makes it actually visible
            screenFlash.color = flashColor;
        }

        float elapsed = 0f;
        float toggleInterval = 0.1f; // How fast the player sprite blinks (e.g., every 0.1 seconds)
        float timerSinceLastToggle = 0f;

        //The player sprite flash loop

        while (elapsed < invulnerabilityDuration)
        {

            // timers by the time passed since the last frame

            elapsed += Time.deltaTime;
            timerSinceLastToggle += Time.deltaTime;

            // Toggle player sprite visibility at set intervals

            if (timerSinceLastToggle >= toggleInterval)

            {

                if (playerSprite != null)

                {

                    Color spriteColor = playerSprite.color;

                    spriteColor.a = (spriteColor.a == 1f) ? 0.3f : 1f;

                    playerSprite.color = spriteColor;
                }
        
                timerSinceLastToggle = 0f; // Reset the toggle timer
            }
        
            // Turn off the screen flash early if flashDuration is reached

            if (screenFlash != null && elapsed >= flashDuration)
            {
                Color finalFlashColor = screenFlash.color;
                finalFlashColor.a = 0f; // makes it invisible again
                screenFlash.color = finalFlashColor;
            }

        

            // Yield control back to Unity until the next frame

            yield return null;
        }

        // Makes sure the screen flash is off

        if (screenFlash != null)
        {
            Color finalFlashColor = screenFlash.color;
            finalFlashColor.a = 0f;
            screenFlash.color = finalFlashColor;
        }
        
        // Makes sure the sprite is fully visible again
        if (playerSprite != null)
        {
            Color finalSpriteColor = playerSprite.color;
            finalSpriteColor.a = 1f;
            playerSprite.color = finalSpriteColor;
        }

        isInvulnerable = false; // makes the player vulnerable again
    }
}