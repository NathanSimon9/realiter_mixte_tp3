using System.Collections;
using System.Collections.Generic;
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
    private float delaiSecurite = 1f;

    [Header("Récompense")]
    public GameObject objetCle;

    [Header("Glisse les paires OFF et ON ici")]
    public GemPair[] pairesDeGems;

    [Header("Audio")]
    public AudioClip sonSuccesEtape; // Le son quand la bouteille est bonne
    public AudioClip sonErreurSequence; // Le son quand on se trompe
    private AudioSource audioSource;

    private void Start()
    {
        // Initialisation de l'audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        ResetVisuels();

        if (objetCle != null) objetCle.SetActive(false);
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
        if (bouteillesCount < 0 || bouteillesCount >= validation.Length) return;

        if (validation[bouteillesCount] == couleur)
        {
            // --- BONNE BOUTEILLE ---
            if (bouteillesCount < pairesDeGems.Length)
            {
                pairesDeGems[bouteillesCount].gemOff.SetActive(false);
                pairesDeGems[bouteillesCount].gemOn.SetActive(true);
            }

            // On joue le son de succès
            if (sonSuccesEtape != null) audioSource.PlayOneShot(sonSuccesEtape);

            bouteillesCount++;
            Debug.Log("Bonne bouteille ! Etape : " + bouteillesCount);
            VerifierReussite();
        }
        else
        {
            // --- ERREUR ---
            Debug.Log("Erreur ! Séquence réinitialisée.");

            // On joue le son d'erreur
            if (sonErreurSequence != null) audioSource.PlayOneShot(sonErreurSequence);

            ResetVisuels();
        }
    }

    private void VerifierReussite()
    {
        if (bouteillesCount >= validation.Length && !estDejaOuvert)
        {
            estDejaOuvert = true;
            Debug.Log("POTIONS REUSSI !");

            if (objetCle != null)
            {
                objetCle.SetActive(true);
                Debug.Log("La clé est apparue !");
            }
        }
    }

    private void ResetVisuels()
    {
        bouteillesCount = 0;
        prochainTempsAcceptable = Time.time + delaiSecurite;

        foreach (GemPair paire in pairesDeGems)
        {
            if (paire.gemOff != null) paire.gemOff.SetActive(true);
            if (paire.gemOn != null) paire.gemOn.SetActive(false);
        }
    }
}