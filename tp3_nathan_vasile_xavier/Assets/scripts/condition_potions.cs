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
        if (Time.time < prochainTempsAcceptable) return;

        if (other.CompareTag("red"))
        {
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
        // SECURITÉ CRITIQUE : On vérifie si l'index est valide avant de faire quoi que ce soit
        if (bouteillesCount < 0 || bouteillesCount >= validation.Length) return;

        if (validation[bouteillesCount] == couleur)
        {
            // Verifier que l'index existe aussi dans le tableau des gemmes
            if (bouteillesCount < pairesDeGems.Length)
            {
                pairesDeGems[bouteillesCount].gemOff.SetActive(false);
                pairesDeGems[bouteillesCount].gemOn.SetActive(true);
            }

            bouteillesCount++;
            Debug.Log("Bonne bouteille ! Etape : " + bouteillesCount);
            VerifierReussite();
        }
        else
        {
            Debug.Log("Erreur ! Séquence réinitialisée.");
            ResetVisuels();
        }
    }

    private void VerifierReussite()
    {
        if (bouteillesCount >= validation.Length && !estDejaOuvert)
        {
            estDejaOuvert = true;
            Debug.Log("POTIONS REUSSI!");
        }
    }

    private void ResetVisuels()
    {
        bouteillesCount = 0;
        // On force un délai de sécurité plus long au reset pour éviter que la 
        // bouteille qui a causé l'erreur ne re-déclenche le script instantanément
        prochainTempsAcceptable = Time.time + delaiSecurite;

        foreach (GemPair paire in pairesDeGems)
        {
            if (paire.gemOff != null) paire.gemOff.SetActive(true);
            if (paire.gemOn != null) paire.gemOn.SetActive(false);
        }
    }
}