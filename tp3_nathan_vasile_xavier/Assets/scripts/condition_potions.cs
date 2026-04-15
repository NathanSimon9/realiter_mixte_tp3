using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class condition_potions : MonoBehaviour
{
    [System.Serializable]
    public struct GemPair
    {
        public string nomEtape;
        public GameObject gemOff;
        public GameObject gemOn;
    }

    private int[] validation = { 1, 2, 1, 1, 2 };
    private int bouteillesCount = 0;
    private bool estDejaOuvert = false;

    // --- SECURITE ---
    private float prochainTempsAcceptable = 0f;
    private float delaiSecurite = 3f;

    [Header("Glisse les paires OFF et ON ici")]
    public GemPair[] pairesDeGems;

    private void Start()
    {
        ResetVisuels();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (estDejaOuvert) return;

        // On vérifie si assez de temps s'est écoulé (3 secondes)
        if (Time.time < prochainTempsAcceptable) return;

        if (other.CompareTag("red"))
        {
            // On définit le prochain moment où on acceptera une bouteille
            prochainTempsAcceptable = Time.time + delaiSecurite;
            TraiterEntree(1);
        }
        else if (other.CompareTag("green"))
        {
            prochainTempsAcceptable = Time.time + delaiSecurite;
            TraiterEntree(2);
        }
    }

    private void TraiterEntree(int couleur)
    {
        if (validation[bouteillesCount] == couleur)
        {
            pairesDeGems[bouteillesCount].gemOff.SetActive(false);
            pairesDeGems[bouteillesCount].gemOn.SetActive(true);

            bouteillesCount++;
            Debug.Log("Bonne bouteille ! Etape : " + bouteillesCount);

            VerifierReussite();
        }
        else
        {
            Debug.Log("Erreur ! On recommence.");
            ResetVisuels();
        }
    }

    /* ANCIEN CODE GARDE EN COMMENTAIRE
    private void OnTriggerEnter(Collider other)
    {
        if (estDejaOuvert) return;
        if (other.tag == "red")
        {
            if (validation[bouteillesCount] == 1)
            {
                bouteillesCount++;
                Debug.Log("RougeOk");
                VerifierReussite();
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
                VerifierReussite();
            }
            else
            {
                bouteillesCount = 0;
                Debug.Log("VertPasOk");
            }
        }
    } 
    */

    private void VerifierReussite()
    {
        if (bouteillesCount == validation.Length && !estDejaOuvert)
        {
            estDejaOuvert = true;
            Debug.Log("POTIONS REUSSI!");
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