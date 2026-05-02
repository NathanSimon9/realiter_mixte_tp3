using UnityEngine;

public class MetalDoorSound : MonoBehaviour
{
    [Header("Paramètres Audio")]
    public AudioClip metalSqueakSound; // La case pour glisser ton son

    private AudioSource audioSource;

    void Start()
    {
        // On récupère le composant AudioSource sur l'objet
        audioSource = GetComponent<AudioSource>();

        // On assigne le clip que tu as glissé dans la case au composant
        audioSource.clip = metalSqueakSound;
        audioSource.loop = true; // On s'assure que le son boucle
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.Stop();
        }
    }
}