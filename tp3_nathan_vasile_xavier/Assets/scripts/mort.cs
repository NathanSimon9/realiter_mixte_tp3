using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class VRDeathHandler : MonoBehaviour
{
    [Header("UI (Placé devant les yeux en VR)")]
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip impactSound;
    public AudioClip deathSound;

    private bool isDying = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isDying) return;

        // Vérifie si l'objet qui touche le piège est le Player (ou sa caméra)
        if (other.transform.root.CompareTag("Player") || other.CompareTag("MainCamera"))
        {
            // Vérifie le tag de l'objet porteur de ce script
            if (gameObject.CompareTag("sol_spike") ||
                gameObject.CompareTag("scie") ||
                gameObject.CompareTag("grinder_triple") ||
                gameObject.CompareTag("axe_swing") ||
                gameObject.CompareTag("Lava")) // Tag Lava conservé
            {
                StartCoroutine(VRDeathSequence());
            }
        }
    }

    // Au cas où tes scies/grinders utilisent des collisions physiques (pas trigger)
    private void OnCollisionEnter(Collision collision)
    {
        if (isDying) return;

        if (collision.transform.root.CompareTag("Player"))
        {
            if (gameObject.CompareTag("sol_spike") ||
                gameObject.CompareTag("scie") ||
                gameObject.CompareTag("grinder_triple") ||
                gameObject.CompareTag("axe_swing") ||
                gameObject.CompareTag("Lava"))
            {
                StartCoroutine(VRDeathSequence());
            }
        }
    }

    IEnumerator VRDeathSequence()
    {
        isDying = true;

        // Son immédiat du piège
        if (audioSource != null && impactSound != null)
            audioSource.PlayOneShot(impactSound);

        // On lance le fondu au noir (indispensable en VR pour éviter le malaise)
        yield return StartCoroutine(FadeToBlack());

        // Son de "Game Over" final
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        // Petit délai pour laisser respirer le joueur dans le noir
        yield return new WaitForSecondsRealtime(0.6f);

        // Recharge le niveau actuel
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator FadeToBlack()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);
            if (fadeImage != null)
                fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}