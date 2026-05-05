using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LavaDeath : MonoBehaviour
{
    [Header("UI")]
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip burnSound;
    public AudioClip deathSound;

    private bool isDying = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isDying) return;

        if (other.transform.root.CompareTag("Player"))
        {
            StartCoroutine(DeathSequence());
        }
    }

    IEnumerator DeathSequence()
    {
        isDying = true;

        // 🔥 son de brûlure (immédiat)
        if (audioSource != null && burnSound != null)
            audioSource.PlayOneShot(burnSound);

        // Pause gameplay
        Time.timeScale = 0f;

        // Fade
        yield return StartCoroutine(FadeToBlack());

        // 💀 son de mort (juste avant reload)
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        // petit délai pour entendre le son
        yield return new WaitForSecondsRealtime(0.3f);

        // Reset + reload
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
}