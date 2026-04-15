using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class condition_potions : MonoBehaviour
{
    private int[] validation = { 1, 2, 1, 1, 2 };
    private int bouteillesCount;
    private bool estDejaOuvert = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "red")
        {
            if (validation[bouteillesCount] == 1)
            {
                bouteillesCount++;
                Debug.Log("RougeOk");
                VerifierReussite(); // On vérifie si c'était la dernière étape
            }
            else
            {
                bouteillesCount = 0;
                Debug.Log("RougePasOk");
            }
        }
        else if (other.tag == "green")
        {
            if (validation[bouteillesCount] == 2)
            {
                bouteillesCount++;
                Debug.Log("VertOk");
                VerifierReussite(); // On vérifie si c'était la dernière étape
            }
            else
            {
                bouteillesCount = 0;
                Debug.Log("VertPasOk");
            }
        }
    }

    private void VerifierReussite()
    {
        // On vérifie si le compte est bon ET si on ne l'a pas déjà fait
        if (bouteillesCount == validation.Length && !estDejaOuvert)
        {
            estDejaOuvert = true; // On verrouille immédiatement
            Debug.Log("Réussite ! Activation de la portekey.");
            ActiverAnimationPortekey();
        }
    }

    private void ActiverAnimationPortekey()
    {
        GameObject portekey = GameObject.FindWithTag("portekey");

        if (portekey != null)
        {
            Animator anim = portekey.GetComponent<Animator>();
            if (anim != null)
            {
                // Joue l'animation une seule fois
                anim.SetTrigger("Ouvrir");
            }
        }
    }
}