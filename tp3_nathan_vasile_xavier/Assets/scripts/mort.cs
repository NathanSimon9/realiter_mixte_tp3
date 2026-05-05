using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LavaDeath : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    private bool isDying = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isDying) return;

        if (other.transform.root.CompareTag("Player"))
        {
            // ➕ AJOUT IMPORTANT (dégâts vie)
            DamagePlayer(other.transform.root.gameObject);

            StartCoroutine(DeathSequence());
        }
    }

    IEnumerator DeathSequence()
    {
        isDying = true;

        // Pause gameplay
        Time.timeScale = 0f;

        // Fade (IMPORTANT : utiliser unscaled time)
        yield return StartCoroutine(FadeToBlack());

        // Reload scene
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

    // ➕ AJOUT POUR SYSTÈME DE VIE
    public void DamagePlayer(GameObject player)
    {
        PlayerLives lives = player.GetComponent<PlayerLives>();

        if (lives != null)
        {
            lives.TakeDamage(1);

            // si mort → on déclenche la vraie mort (fade + reload)
            if (lives.currentLives <= 0)
            {
                StartCoroutine(DeathSequence());
            }
        }
    }
}