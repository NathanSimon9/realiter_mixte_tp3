using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class VRTrapDeath3 : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip deathSound;

    private bool hasHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            StartCoroutine(DeathSequence());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        if (collision.gameObject.CompareTag("Player") || collision.transform.root.CompareTag("Player"))
        {
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        hasHit = true;

        if (audioSource != null)
        {
            if (hitSound != null) audioSource.PlayOneShot(hitSound);
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
        SceneManager.LoadScene(1); // Retour à la scène 1
    }
}