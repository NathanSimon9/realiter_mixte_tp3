using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class condition_potions : MonoBehaviour
{
    private int[] validation = { 1, 2, 1, 1, 2 };
    private int bouteillesCount = 0;
    private bool sequenceTerminee = false; // Empêche de relancer le code après la réussite

    private void OnTriggerEnter(Collider other)
    {
        // Si la porte est déjà ouverte, on ne fait plus rien
        if (sequenceTerminee) return;

        if (other.CompareTag("red"))
        {
            TraiterEntree(1);
        }
        else if (other.CompareTag("green"))
        {
            TraiterEntree(2);
        }
    }

    private void TraiterEntree(int codeCouleur)
    {
        if (validation[bouteillesCount] == codeCouleur)
        {
            bouteillesCount++;
            Debug.Log("Étape " + bouteillesCount + " validée !");

            if (bouteillesCount >= validation.Length)
            {
                OuvrirPorte();
            }
        }
        else
        {
            // Erreur dans la séquence : on réinitialise tout
            bouteillesCount = 0;
            Debug.Log("Erreur ! La séquence repart à zéro.");
        }
    }

    private void OuvrirPorte()
    {
        sequenceTerminee = true; // Verrouille la réussite

        GameObject portekey = GameObject.FindWithTag("portekey");

        if (portekey != null)
        {
            Animator anim = portekey.GetComponent<Animator>();
            if (anim != null)
            {
                // On active le Trigger pour lancer l'animation
                anim.SetTrigger("Ouvrir");
                Debug.Log("Animation de la porte lancée !");
            }
            else
            {
                Debug.LogWarning("L'objet 'portekey' n'a pas d'Animator !");
            }
        }
    }
}