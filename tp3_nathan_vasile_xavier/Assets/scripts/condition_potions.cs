using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class condition_potions : MonoBehaviour
{
    [System.Serializable]
    public struct GemPair
    {
        public string nomEtape; // Juste pour s'y retrouver dans l'inspecteur
        public GameObject gemOff;
        public GameObject gemOn;
    }
    private int[] validation = { 1, 2, 1, 1, 2 };
    private int bouteillesCount;
    private bool estDejaOuvert = false;

    [Header("Glisse les paires OFF et ON ici")]
    public GemPair[] pairesDeGems;

    private void Start()
    {
        ResetVisuels();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (estDejaOuvert) return;

        // On vérifie la couleur rouge
        if (other.CompareTag("red"))
        {
            TraiterEntree(1);
            // Optionnel : on peut détruire la bouteille ou la téléporter pour éviter qu'elle retouche le trigger
            Destroy(other.gameObject);
        }
        // On vérifie la couleur verte
        else if (other.CompareTag("green"))
        {
            TraiterEntree(2);
            Destroy(other.gameObject);
        }
    }

    private void TraiterEntree(int couleur)
    {
        if (validation[bouteillesCount] == couleur)
        {
            // --- AMÉLIORATION VISUELLE ---
            // On allume la gemme AVANT de changer l'index
            pairesDeGems[bouteillesCount].gemOff.SetActive(false);
            pairesDeGems[bouteillesCount].gemOn.SetActive(true);

            bouteillesCount++;
            Debug.Log("Couleur OK ! Étape : " + bouteillesCount);

            VerifierReussite();
        }
        else
        {
            Debug.Log("Erreur ! Séquence brisée.");
            ResetVisuels(); // On appelle ta fonction qui remet bouteillesCount à 0
        }
    }

    /* private void OnTriggerEnter(Collider other)
    {

        if (estDejaOuvert) return;

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
    }  }  */
    private void VerifierReussite()
    {
        if (bouteillesCount == validation.Length && !estDejaOuvert)
        {
            estDejaOuvert = true;
            Debug.Log("POTIONS REUSSI!");
            Debug.Log("REUSSITE DES POTIONS!");
        }
    }
    private void ResetVisuels()
    {
        bouteillesCount = 0;
        foreach (GemPair paire in pairesDeGems)
        {
            if (paire.gemOff != null) paire.gemOff.SetActive(true);
            if (paire.gemOn != null) paire.gemOn.SetActive(false);
        }
    }

}