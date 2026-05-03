using UnityEngine;
using System.Collections; // Obligatoire pour utiliser les Coroutines (Wait)

public class Porte_metal_son : MonoBehaviour
{
    [Header("Paramètres Audio")]
    public AudioClip metalSqueakSound;
    public float delaiAvantRejouer = 2.0f; // Le temps d'attente de 2 sec

    private AudioSource audioSource;
    private bool peutJouer = true; // Pour gérer le temps d'attente

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = metalSqueakSound;
        audioSource.playOnAwake = false;
        audioSource.loop = false; // On ne veut pas qu'il boucle indéfiniment
    }

    // On utilise OnTriggerEnter car tu as parlé de "Trigger"
    private void OnTriggerEnter(Collider other)
    {
        // On vérifie si c'est le joueur et si on a le droit de jouer le son
        if (other.CompareTag("Player") && peutJouer)
        {
            StartCoroutine(JouerSonEtAttendre());
        }
    }

    IEnumerator JouerSonEtAttendre()
    {
        peutJouer = false; // On bloque le son pour ne pas qu'il se répète

        Debug.Log("<color=cyan>🔊 Son lancé !</color>");
        audioSource.Play();

        // On attend que le son finisse, plus 1 seconde supplémentaire comme demandé
        // Si ton son fait 2 sec, il attendra 3 sec au total
        yield return new WaitForSeconds(audioSource.clip.length + 1.0f);

        Debug.Log("<color=orange>⌛ Fin du son (+1s). Début du délai d'attente...</color>");

        // On attend les 2 secondes de sécurité avant de pouvoir recommencer
        yield return new WaitForSeconds(delaiAvantRejouer);

        peutJouer = true; // On autorise à nouveau le son
        Debug.Log("<color=green>✅ Prêt à rejouer !</color>");
    }
}