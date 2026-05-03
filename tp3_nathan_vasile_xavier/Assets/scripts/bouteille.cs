using UnityEngine;

public class InteractionCouleurs : MonoBehaviour
{
    [Header("Audio Unique")]
    public AudioClip sonBouteille; // Une seule case pour le son

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.clip = sonBouteille;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // On récupère le tag de l'objet touché
        string tagTouche = collision.gameObject.tag;

        // On garde les "if" pour savoir ce qu'on a touché dans la console
        if (tagTouche == "red")
        {
            audioSource.Play();
            Debug.Log("<color=red>🔴 Impact sur du ROUGE : Son joué !</color>");
        }
        else if (tagTouche == "green")
        {
            audioSource.Play();
            Debug.Log("<color=green>🟢 Impact sur du VERT : Son joué !</color>");
        }
    }
}