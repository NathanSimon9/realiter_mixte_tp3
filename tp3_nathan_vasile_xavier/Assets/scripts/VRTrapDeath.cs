using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class VRTrapDeath : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fadeImage; // Glisse ton image noire du Canvas ici
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hitSound;   // Son de l'impact (scie, pics, etc.)
    public AudioClip deathSound; // Son de mort global

    private bool isDying = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isDying) return;

        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            CheckTagAndDie();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDying) return;

        if (collision.gameObject.CompareTag("Player") || collision.transform.root.CompareTag("Player"))
        {
            CheckTagAndDie();
        }
    }

    void CheckTagAndDie()
    {
        // Vérification des tags spécifiques à ton projet Montmo
        if (gameObject.CompareTag("sol_spike") ||
            gameObject.CompareTag("scie") ||
            gameObject.CompareTag("grinder_triple") ||
            gameObject.CompareTag("axe_swing"))
        {
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        isDying = true;

        // Jouer les sons
        if (audioSource != null)
        {
            if (hitSound != null) audioSource.PlayOneShot(hitSound);
            if (deathSound != null) audioSource.PlayOneShot(deathSound);
        }

        // Geler le jeu pour la VR
        Time.timeScale = 0f;

        // Fondu au noir
        if (fadeImage != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float alpha = Mathf.Clamp01(elapsed / fadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }

        // Petit délai de sécurité en temps réel
        yield return new WaitForSecondsRealtime(0.3f);

        // Remettre le temps à la normale et charger l'index 3
        Time.timeScale = 1f;
        SceneManager.LoadScene(3);
    }
}