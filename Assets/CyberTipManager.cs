// Programmer: Alex W
// This script will manage the cyber tips that display on the top of the screen

using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CyberTipManager : MonoBehaviour
{
    // Singleton pattern to ensure only one instance of the CyberTipManager exists
    public static CyberTipManager instance;

    public TextMeshProUGUI tipText; // Reference to the TextMeshProUGUI component for displaying tips

    public float displayDuration = 3f; // Duration for which the tip will be displayed
    public float fadeDuration = 1f; // Duration for the fade in/out effect

    // Dictionary that matches enemy types to cyber safety tips
    // This will be the main structure, as each key is a different enemy while each value is a tip
    private Dictionary<string, string> tipsByEnemy;


    private string[] generalTips; // Backup array of general tips to display when an enemy type doesn't have a specific tip

    private Coroutine activeTipRoutine; // tracks the current tip so there isn't overlap

    void Awake()
    {
        // Single setup so only one cybertipmanager exists 
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeTips(); // Sets up the tips data structure
    }

    void Start()
    {
        // Makes sure the tip text is hidden at the start
        if (tipText != null)
        {
            tipText.text = ""; // Clear any default text
            tipText.alpha = 0f; // Start with the text invisible
        }
    }

    private void InitializeTips()
    {
        // Dictionary that should match each enemy type to a specific tip.
        tipsByEnemy = new Dictionary<string, string>()
        {
            { "GameObject", "Phishing scams can disguise themselves as trustworthy, so be cautious of unknown and unexpected messages." },
            { "FallingMalware ", "Malware can be hidden in downloads. Only download from trusted sources." },
            { "SpamBomb", "Spam emails usually contain harmful links or attachments. Never open them." },
            { "SurgingVirus", "Viruses spread very fast once inside a system. Keep your computer safe by using antivirus and keeping it updated." }
        }; 

         // General tips if there isn't a specific one for the enemy type
        generalTips = new string[]
        {
            "Never share your passwords with anyone, even your friends.",
            "Always check a websites URL before putting in any information.",
            "If something seems to good to be true, assume it is.",
            "Public wifi isn't secure, so don't log into accounts on it.",
            "Enabling two-factor authentication can add an extra layer of security to your accounts."
        };
    }

    // This is what other scripts will call on to show a tip
    // enemyName should match the name of a enemy
    public void showTip(string enemyName)
    {
        // Doesn't do anything if the enemy name isn't null/empty
        if (string.IsNullOrEmpty(enemyName))

        {
            Debug.LogWarning("CyberTipManager: ShowTip was called with a empty enemy name.");
            return;
        }

        // Checks if the ui is assigned or not
        if (tipText == null)
        {
            Debug.LogError("CyberTipManager: tipText is not assigned in the inspector.");
            return;
        }

        string tipToShow;

        // Checks if enemy is in the dictionary 
        if (tipsByEnemy.ContainsKey(enemyName))
        {
            tipToShow = tipsByEnemy[enemyName]; // uses the specific tip
        }
        else
        {
            int randomIndex = Random.Range(0, generalTips.Length);
            tipToShow = generalTips[randomIndex];
            Debug.Log("CyberTipManager: No specific tip for " + enemyName + ", showing general tip instead.");
        }

        // Stops any tip thats currently showing before it starts a new one
        if (activeTipRoutine != null)
        {
            StopCoroutine(activeTipRoutine);
        }

        activeTipRoutine = StartCoroutine(DisplayTipRoutine(tipToShow));
    }

    private IEnumerator DisplayTipRoutine(string tip)
    {
        // Sets the text to fully visable instantly 
        tipText.text = tip;
        tipText.alpha = 1f;

        // Wauts fir the display duration before fading
        yield return new WaitForSeconds(displayDuration);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            tipText.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration); // Hopefully smooth fade
            yield return null;
        }

        // makes sure its fully hidden the text 
        tipText.alpha = 0f;
        tipText.text = "";
        activeTipRoutine = null;
    }
}
