using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.UI;

using System.Collections;

public class VRLavaDeath : MonoBehaviour

{

    [Header("UI")]

    [SerializeField] private Image fadeImage; // Glisse ton image noire du Canvas ici

    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Audio")]

    public AudioSource audioSource;

    public AudioClip burnSound;  // Le son spécifique à la lave (sizzle)

    public AudioClip deathSound; // Son de mort global

    [Header("Settings")]

    [SerializeField] private float postFadeDelay = 0.3f;

    private bool isDying = false;

    // Détection de la lave

    private void OnTriggerEnter(Collider other)

    {

        if (isDying) return;

        // On vérifie si l'objet qui touche la lave est le Player

        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))

        {

            Debug.Log("🔥 Contact avec la lave !");

            StartCoroutine(DeathSequence());

        }

    }

    // Cette fonction peut aussi être appelée par d'autres scripts si besoin

    public void StartDeathSequence()

    {

        if (!isDying) StartCoroutine(DeathSequence());

    }

    private IEnumerator DeathSequence()

    {

        isDying = true;

        // Jouer les sons de brûlure et de mort

        if (audioSource != null)

        {

            if (burnSound != null) audioSource.PlayOneShot(burnSound);

            if (deathSound != null) audioSource.PlayOneShot(deathSound);

        }

        // Geler le temps pour l'immersion VR

        Time.timeScale = 0f;

        // Lancer le fondu au noir

        if (fadeImage != null)

        {

            float elapsed = 0f;

            while (elapsed < fadeDuration)

            {

                elapsed += Time.unscaledDeltaTime; // Utilise le temps réel car timeScale est à 0

                float alpha = Mathf.Clamp01(elapsed / fadeDuration);

                fadeImage.color = new Color(0, 0, 0, alpha);

                yield return null;

            }

            fadeImage.color = Color.black;

        }

        // Petit délai supplémentaire en temps réel

        yield return new WaitForSecondsRealtime(postFadeDelay);

        // Remettre le temps à la normale pour la scène suivante

        Time.timeScale = 1f;

        // Chargement de la scène à l'index 3
        // Rechargement de la scène actuelle
        Debug.Log("🔄 Rechargement de la scène : " + SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

}
