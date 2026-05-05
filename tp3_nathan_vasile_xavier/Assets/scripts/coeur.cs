using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public PlayerLives player;

    public Image heart1;
    public Image heart2;
    public Image heart3;

    void Update()
    {
        int lives = player.currentLives;

        heart1.enabled = lives >= 1;
        heart2.enabled = lives >= 2;
        heart3.enabled = lives >= 3;
    }
}