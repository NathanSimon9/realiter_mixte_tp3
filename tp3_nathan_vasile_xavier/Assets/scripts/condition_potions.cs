using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class condition_potions : MonoBehaviour
{
    private int[] validation = { 1, 2, 1, 1, 2 };
    private int bouteillesCount;

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
        if (bouteillesCount == validation.Length)
        {
            Debug.Log("Réussite ! Activation de la portekey.");
            ActiverAnimationPortekey();
        }
    }

    private void ActiverAnimationPortekey()
    {
        // On cherche l'objet avec le tag "portekey"
        GameObject portekey = GameObject.FindWithTag("portekey");

        if (portekey != null)
        {
            Animator anim = portekey.GetComponent<Animator>();
            if (anim != null)
            {
                // Remplace "Ouvrir" par le nom exact de ton paramètre Trigger dans l'Animator
                anim.SetTrigger("Ouvrir");
            }
            else
            {
                Debug.LogWarning("L'objet portekey n'a pas de composant Animator.");
            }
        }
        else
        {
            Debug.LogError("Aucun objet avec le tag 'portekey' n'a été trouvé dans la scène.");
        }
    }
}