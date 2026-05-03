using UnityEngine;
using System.Collections; // Nécessaire pour les Coroutines

public class InteractionCouleurs : MonoBehaviour
{
    [Header("Audio Unique")]
    public AudioClip sonBouteille;

    private AudioSource audioSource;
    private bool peutJouer = true; // Pour éviter de relancer le délai si on touche déjà quelque chose

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

        if ((tagTouche == "red" || tagTouche == "green") && peutJouer)
        {
            StartCoroutine(JouerSonAvecDelai(tagTouche));
        }
    }

    IEnumerator JouerSonAvecDelai(string tag)
    {
        peutJouer = false; // On bloque les autres entrées pendant le délai

        // --- DÉLAI DE 2 SECONDES ---
        yield return new WaitForSeconds(2f);

        audioSource.Play();

        if (tag == "red")
            Debug.Log("<color=red>🔴 Impact ROUGE (après 2s) !</color>");
        else
            Debug.Log("<color=green>🟢 Impact VERT (après 2s) !</color>");

        peutJouer = true; // On autorise à nouveau le son après la lecture
    }
}