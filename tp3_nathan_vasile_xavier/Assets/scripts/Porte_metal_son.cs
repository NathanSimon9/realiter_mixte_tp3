using UnityEngine;

// Cette ligne force Unity à ajouter un AudioSource si il n'y en a pas
[RequireComponent(typeof(AudioSource))]
public class Porte_metal_son : MonoBehaviour
{
    [Header("Paramètres Audio")]
    public AudioClip metalSqueakSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (metalSqueakSound != null)
        {
            audioSource.clip = metalSqueakSound;
            // On s'assure que le son boucle pour un grincement continu
            audioSource.loop = true;
            Debug.Log("<color=green>✔ Son chargé avec succès : </color>" + metalSqueakSound.name);
        }
        else
        {
            Debug.LogError("<color=red>✘ ERREUR : Glisse un son dans la case 'Metal Squeak Sound' !</color>");
        }
    }

    // Déclenché quand le joueur pousse la porte (contact physique)
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log("<color=cyan>🔊 Son EN LECTURE (Le joueur pousse la porte).</color>");
            }
        }
    }

    // Déclenché quand le joueur s'arrête ou s'éloigne
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log("<color=yellow>🔇 Son ARRÊTÉ (Le joueur a lâché la porte).</color>");
            }
        }
    }
}