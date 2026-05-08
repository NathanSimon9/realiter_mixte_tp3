using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class VRTrapDeath : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip deathSound;

    [Header("Référence caméra VR (mets ta Main Camera ici)")]
    public Transform cameraVR;

    [Header("Fade")]
    public Image fadeImage;
    public float fadeDuration = 1.5f;

    private bool isDying = false;

    void Start()
    {
        // Assure que le fade commence transparent
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDying) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(DeathSequence());
        }
    }

    IEnumerator DeathSequence()
    {
        isDying = true;

        // 🔊 Joue le son DIRECTEMENT à la position de la caméra VR
        if (deathSound != null && cameraVR != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, cameraVR.position, 1f);
        }

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                fadeImage.color = c;
            }

            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
