using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class VRLavaDeath2 : MonoBehaviour
{
    [SerializeField] private Image fadeImage; // Glisse l'image noire de ton Canvas ici
    public AudioSource audioSource;
    public AudioClip deathSound;
    public AudioClip burnSound;

    private bool isDying = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isDying) return;

        // Détection du joueur
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        isDying = true;

        // Sons de brûlure et de mort
        if (audioSource != null)
        {
            if (burnSound != null) audioSource.PlayOneShot(burnSound);
            if (deathSound != null) audioSource.PlayOneShot(deathSound);
        }

        Time.timeScale = 0f;

        if (fadeImage != null)
        {
            float elapsed = 0f;
            while (elapsed < 0.5f)
            {
                elapsed += Time.unscaledDeltaTime;
                fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(elapsed / 0.5f));
                yield return null;
            }
        }

        yield return new WaitForSecondsRealtime(0.3f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(4); // Charge la scène 4
    }
}