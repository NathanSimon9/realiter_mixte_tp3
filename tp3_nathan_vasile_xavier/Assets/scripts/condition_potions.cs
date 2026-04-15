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