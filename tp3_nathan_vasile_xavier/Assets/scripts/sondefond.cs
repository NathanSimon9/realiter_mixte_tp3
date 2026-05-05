using UnityEngine;

public class MusiqueFondScene : MonoBehaviour
{
    public AudioClip musiquefondscene01;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = musiquefondscene01;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // son 2D (fond)

        Debug.Log("🎵 Musique prête");
    }

    void Start()
    {
        if (musiquefondscene01 == null)
        {
            Debug.LogError("❌ AudioClip non assigné !");
            return;
        }

        Debug.Log("▶️ Musique de fond lancée");
        audioSource.Play();
    }

    void OnDestroy()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            Debug.Log("⛔ Arrêt musique (scene quit)");
            audioSource.Stop();
        }
    }
}