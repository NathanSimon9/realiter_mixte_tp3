using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives;

    // 🔥 PERSISTANCE ENTRE SCÈNES
    public static int savedLives = 3;

    void Start()
    {
        currentLives = savedLives;
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;

        if (currentLives < 0)
            currentLives = 0;

        // 💾 sauvegarde avant reload
        savedLives = currentLives;
    }
}