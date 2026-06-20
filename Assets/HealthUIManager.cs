using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    [SerializeField] private GameObject heartv2;
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    private List<Image> spawnedHearts = new List<Image>();

    public void InitializeHearts(int maxHealth)
    {
        // Safe backward loop + DestroyImmediate stops Unity 6 editor crashes
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        spawnedHearts.Clear();

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newHeart = Instantiate(heartv2, transform, false);
            Image heartImage = newHeart.GetComponent<Image>();
            if (heartImage != null)
            {
                heartImage.sprite = fullHeartSprite;
                spawnedHearts.Add(heartImage);
            }
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < spawnedHearts.Count; i++)
        {
            // Defensive Programming: Skips if a heart was missing/destroyed
            if (spawnedHearts[i] == null) continue;

            if (i < currentHealth)
            {
                spawnedHearts[i].sprite = fullHeartSprite;
            }
            else
            {
                spawnedHearts[i].sprite = emptyHeartSprite;
            }
        }
    }
}