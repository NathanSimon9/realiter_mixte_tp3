using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Pour gérer la liste des contacts

public class InteractionCouleurs : MonoBehaviour
{
    [Header("Audio Unique")]
    public AudioClip sonBouteille;

    private AudioSource audioSource;
    private bool peutJouer = true;

    // Liste pour suivre les objets red/green actuellement touchés
    private List<GameObject> objetsEnContact = new List<GameObject>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.clip = sonBouteille;
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tagTouche = collision.gameObject.tag;

        if (tagTouche == "red" || tagTouche == "green")
        {
            // On ajoute l'objet à notre liste de contacts
            if (!objetsEnContact.Contains(collision.gameObject))
            {
                objetsEnContact.Add(collision.gameObject);
            }

            // On lance la coroutine si elle n'est pas déjà en cours
            if (peutJouer)
            {
                StartCoroutine(JouerSonSiToujoursEnContact(tagTouche));
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Quand on ne touche plus l'objet, on le retire de la liste
        if (objetsEnContact.Contains(collision.gameObject))
        {
            objetsEnContact.Remove(collision.gameObject);
        }
    }

    IEnumerator JouerSonSiToujoursEnContact(string dernierTag)
    {
        peutJouer = false;

        // --- ATTENTE DE 2 SECONDES ---
        yield return new WaitForSeconds(2f);

        // --- VÉRIFICATION FINALE ---
        // Si la liste n'est pas vide, cela veut dire qu'on touche encore au moins un objet valide
        if (objetsEnContact.Count > 0)
        {
            audioSource.Play();

            if (dernierTag == "red")
                Debug.Log("<color=red>🔴 Toujours en contact : Son ROUGE joué !</color>");
            else
                Debug.Log("<color=green>🟢 Toujours en contact : Son VERT joué !</color>");
        }
        else
        {
            Debug.Log("⏳ Contact rompu avant les 2s : Le son ne joue pas.");
        }

        peutJouer = true;
    }
}