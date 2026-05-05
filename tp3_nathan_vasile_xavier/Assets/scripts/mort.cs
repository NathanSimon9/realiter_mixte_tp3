using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LavaDeath : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip lavaSound;
    public AudioClip deathSound;

    private bool isDying = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isDying) return;

        if (other.transform.root.CompareTag("Player"))
        {
            PlayerLives player =
                other.transform.root.GetComponent<PlayerLives>();

            if (player != null)
            {
                // 🔥 son de lave (impact immédiat)
                PlaySound(lavaSound);

                player.TakeDamage(1);

                if (player.currentLives <= 0)
                {
                    StartCoroutine(DeathSequence());
                }
            }
        }
    }

    IEnumerator DeathSequence()
    {
        isDying = true;

        // 💀 son de mort
        PlaySound(deathSound);

        Time.timeScale = 0f;

        yield return StartCoroutine(FadeToBlack());

        Time.timeScale = 1f;

        SceneManager.LoadScene("Menu");
    }

    IEnumerator FadeToBlack()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;

            float alpha = t / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);

            yield return null;
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}