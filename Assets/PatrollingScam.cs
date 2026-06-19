using UnityEngine;

public class PatrollingScam : Hazard
{
    public float speed = 2f; // speed of enemy
    public float patrolDistance = 3f; // how far the enemy can patrol

    private Vector2 startPosition; // where they start patrolling from
    private int direction = 1;

    void Start()
    {
        startPosition = transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {   // moves the enemy back and forth every frame based on its local parameters
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime); // moves the enemy

        if (Vector2.Distance(startPosition, transform.position) >= patrolDistance)
        {
            direction *= -1; // Changes direction
    
        // CALCULATE EXACT TURN POINT: Multiply the normalized direction by the intended patrol distance
        Vector2 travelDirection = (Vector2)transform.position - startPosition;
        startPosition = startPosition + travelDirection.normalized * patrolDistance;
    
        // Reset the enemy's position to the exact turn point to prevent overshooting
        transform.position = startPosition; 
        }

    }
}
