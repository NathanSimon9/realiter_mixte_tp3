using UnityEngine;
using System.Collections;

public class Porte_metal_son : MonoBehaviour
{
    [Header("Paramètres Audio")]
    public AudioClip metalSqueakSound;
    public float delaiAvantRejouer = 2.0f;

    private bool peutJouer = true;

    private void OnTriggerEnter(Collider other)
    {
        // On vérifie si c'est le joueur
        if (other.CompareTag("Player") && peutJouer)
        {
            // On vérifie si l'objet qui a le script a l'un des tags de cellule
            if (gameObject.tag.StartsWith("cel"))
            {
                // On cherche l'AudioSource sur la porte (souvent un enfant du trigger ou l'objet lui-même)
                AudioSource sourcePorte = GetComponentInChildren<AudioSource>();

                if (sourcePorte != null)
                {
                    StartCoroutine(JouerSonEtAttendre(sourcePorte));
                }
                else
                {
                    Debug.LogWarning("Aucun AudioSource trouvé sur " + gameObject.name + " ou ses enfants !");
                }
            }
        }
    }

    IEnumerator JouerSonEtAttendre(AudioSource source)
    {
        peutJouer = false;

        Debug.Log("<color=cyan>🔊 Grincement lancé sur : </color>" + gameObject.tag);

        // On joue le son unique
        source.PlayOneShot(metalSqueakSound);

        // Attente : longueur du son + 1 seconde
        yield return new WaitForSeconds(metalSqueakSound.length + 1.0f);

        // Délai de sécurité
        yield return new WaitForSeconds(delaiAvantRejouer);

        peutJouer = true;
        Debug.Log("<color=green>✅ Prêt pour le prochain grincement !</color>");
    }
}