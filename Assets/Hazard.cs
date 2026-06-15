// This is the parent class for enemies
using UnityEngine;

public class Hazard : MonoBehaviour
{
  // i will be using the protected keyword so child classes can access this.
  // virtual means child classes can overide things I write. 

    public int damage = 1; // Seing as the player only has 3 health this seems fair

    protected virtual void OnTriggerEnter2D(Collider2D other) // haven't set a name yet i think (this is what they said to put first; 'other')
    {
        // Check if player collided with hazard
        if (other.CompareTag("Player"))
        {
            Debug.Log("HAZARD SYSTEM: Player fell for a cyber attack! " + gameObject.name); // for testing


            //we gotta get the player script so we can say that the player takes damage
            playerMovement player = other.GetComponent<playerMovement>();
            
            // if the player script is found we trigger the health loss 
            if (player != null)
            {
                player.TakeDamage(damage);

                // Tell the CyberTipManager which enemy hit the player so it can show the right tip
                if (CyberTipManager.instance != null)
                {
                    CyberTipManager.instance.showTip(gameObject.GetType().Name);
                }
            }
            // will get the player take damage 
            HandleDestruction();
        }
    }

    protected virtual void HandleDestruction()
    {
        Destroy(gameObject); // gets rid of it
    }
}
