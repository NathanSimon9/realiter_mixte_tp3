using UnityEngine;

public class BouteilleInteraction : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip sonBouteille;
    private AudioSource audioSource;

    void Start()
    {
        // On récupère ou on ajoute un AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.clip = sonBouteille;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // On vérifie les tags des objets touchés
        string tagTouche = collision.gameObject.tag;

        if (tagTouche == "Player" || tagTouche == "red" || tagTouche == "green")
        {
            // On joue le son s'il n'est pas déjà en train de jouer
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log("<color=white>🍾 Contact avec : </color>" + tagTouche);
            }
        }
    }
}