using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class VRLavaDeath : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathSound;
    public AudioClip burnSound; // Ajouté du 2e script

    [Header("Settings")]
    [SerializeField] private float postFadeDelay = 0.3f;
    [SerializeField] private bool verboseLogs = true;

    private bool isDying = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isDying) return;

        // On vérifie le tag du Player (ou de sa racine pour la VR)
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            Debug.Log("🔥 Mort par la lave ! Objet : " + other.name);
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        isDying = true;

        if (verboseLogs) Debug.Log($"[LavaDeath] Début de la séquence à {Time.realtimeSinceStartup:F3}s");

        // Jouer les sons
        PlaySoundSafe(burnSound);
        PlaySoundSafe(deathSound);

        // Geler le temps (pratique pour éviter que le joueur continue de bouger en VR)
        Time.timeScale = 0f;

        // Lancer le fondu au noir
        if (fadeImage != null)
        {
            yield return StartCoroutine(FadeToBlack());
        }

        // Petit délai supplémentaire en temps réel pour l'immersion/le son
        yield return new WaitForSecondsRealtime(postFadeDelay);

        // Remettre le temps à la normale avant de recharger
        Time.timeScale = 1f;

        if (verboseLogs) Debug.Log("🔄 Reload de la scène.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void PlaySoundSafe(AudioClip clip)
    {
        if (clip == null) return;

        if (audioSource != null)
        {
            audioSource.ignoreListenerPause = true; // Pour que le son joue même si le temps est figé
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("🔊 AudioSource manquant !");
        }
    }

    private IEnumerator FadeToBlack()
    {
        float elapsed = 0f;
        Color startColor = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // Utilise le temps réel car timeScale est à 0
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        fadeImage.color = Color.black;
    }
}