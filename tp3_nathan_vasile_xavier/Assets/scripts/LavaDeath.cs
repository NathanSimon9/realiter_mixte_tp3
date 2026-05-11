using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LavaDeath : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fadeImage;
    [Tooltip("Duration of the fade in seconds (real time).")]
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip burnSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Scene")]
    [Tooltip("Scene name to reload when the player dies. If empty, the scene that contains this lava object will be used.")]
    [SerializeField] private string sceneToReload = "scene_pont";

    [Header("Timing")]
    [Tooltip("Extra real-time delay after fade before reloading the scene.")]
    [SerializeField] private float postFadeDelay = 0.3f;

    [Header("Debug")]
    [SerializeField] private bool verboseLogs = false;

    private bool isDying;

    private void OnTriggerEnter(Collider other)
    {
        if (isDying) return;

        if (other.transform.root.CompareTag("Player"))
        {
            // Determine which scene name to reload: inspector override or the scene that contains this lava object
            string sceneNameToReload = string.IsNullOrEmpty(sceneToReload) ? gameObject.scene.name : sceneToReload;

            if (verboseLogs)
            {
                Debug.Log($"[LavaDeath] Active scene: {SceneManager.GetActiveScene().buildIndex} - {SceneManager.GetActiveScene().name}");
                Debug.Log($"[LavaDeath] Lava object scene: {gameObject.scene.buildIndex} - {gameObject.scene.name}");
                Debug.Log($"[LavaDeath] Chosen scene to reload: {sceneNameToReload}");
                if (sceneNameToReload != gameObject.scene.name)
                    Debug.Log($"[LavaDeath] Using inspector override 'sceneToReload' instead of lava object's scene.");
            }

            StartCoroutine(DeathSequence(sceneNameToReload));
        }
    }

    // Pass the scene name to reload so we don't depend on the active scene or build order
    private IEnumerator DeathSequence(string sceneName)
    {
        isDying = true;

        if (verboseLogs) Debug.Log($"[LavaDeath] DeathSequence start at {Time.realtimeSinceStartup:F3}s (reload '{sceneName}')");

        // Play sounds immediately in real time
        PlaySoundSafe(burnSound);
        PlaySoundSafe(deathSound);

        // Freeze game time (useful for VR synchronization)
        Time.timeScale = 0f;
        if (verboseLogs) Debug.Log($"[LavaDeath] Time frozen at {Time.realtimeSinceStartup:F3}s");

        // Start fade using unscaled time
        if (fadeImage != null)
            StartCoroutine(FadeToBlack());

        // Wait for fade to finish in real time
        yield return new WaitForSecondsRealtime(fadeDuration);

        if (verboseLogs) Debug.Log($"[LavaDeath] Fade finished at {Time.realtimeSinceStartup:F3}s");

        // Small real-time delay so sounds can be heard
        yield return new WaitForSecondsRealtime(postFadeDelay);

        // Restore time and reload
        Time.timeScale = 1f;
        if (verboseLogs) Debug.Log($"[LavaDeath] Restoring time and reloading at {Time.realtimeSinceStartup:F3}s");

        // Reload the scene by name (ensure the scene name is added to Build Settings)
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    /// <summary>
    /// Plays a clip via the configured AudioSource or falls back to PlayClipAtPoint.
    /// Ensures playback is triggered in real time even if the AudioListener gets paused.
    /// </summary>
    private void PlaySoundSafe(AudioClip clip)
    {
        if (clip == null) return;

        if (audioSource != null)
        {
            audioSource.ignoreListenerPause = true;
            audioSource.PlayOneShot(clip);
            if (verboseLogs) Debug.Log($"[LavaDeath] Played clip '{clip.name}' at {Time.realtimeSinceStartup:F3}s");
        }
        else
        {
            Vector3 pos = Camera.main != null ? Camera.main.transform.position : Vector3.zero;
            AudioSource.PlayClipAtPoint(clip, pos);
            if (verboseLogs) Debug.Log($"[LavaDeath] Played clip (fallback) '{clip.name}' at {Time.realtimeSinceStartup:F3}s");
        }
    }

    private IEnumerator FadeToBlack()
    {
        if (fadeImage == null)
            yield break;

        float elapsed = 0f;
        Color start = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(elapsed / Mathf.Max(0.0001f, fadeDuration));
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0f, 0f, 0f, 1f);
    }
}