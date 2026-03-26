using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Obligatoire pour changer de scène

public class GestionPotions : MonoBehaviour
{
    [Header("--- UI Textes ---")]
    public TextMeshProUGUI texteCodePotion;
    public TextMeshProUGUI texteCompteurRouges;
    public TextMeshProUGUI texteCompteurVertes;

    [Header("--- Configuration du Code ---")]
    public string codeVictoire = "RVRRV"; // La recette exacte
    public string sceneRetour = "scene_donjon"; // Nom de ta scène de combat

    private int nbRouges = 4;
    private int nbVertes = 4;

    void Start()
    {
        texteCodePotion.text = "";
        MettreAJourInterface();
    }

    public void ActionBoutonRouge()
    {
        if (nbRouges > 0)
        {
            nbRouges--;
            texteCodePotion.text = texteCodePotion.text + "R";
            MettreAJourInterface();
            VerifierCombinaison(); // On vérifie après chaque ajout
        }
    }

    public void ActionBoutonVert()
    {
        if (nbVertes > 0)
        {
            nbVertes--;
            texteCodePotion.text = texteCodePotion.text + "V";
            MettreAJourInterface();
            VerifierCombinaison(); // On vérifie après chaque ajout
        }
    }

    void VerifierCombinaison()
    {
        // Si le texte écrit est EXACTEMENT RVRRV
        if (texteCodePotion.text == codeVictoire)
        {
            Debug.Log("POTION RÉUSSIE ! Retour au donjon...");

            // On charge la scène du donjon
            SceneManager.LoadScene(sceneRetour);
        }
        else if (texteCodePotion.text.Length >= codeVictoire.Length)
        {
            // Si on a tapé assez de lettres mais que c'est faux
            Debug.Log("Mauvaise recette... Réinitialisation !");
            ResetPotion();
        }
    }

    void ResetPotion()
    {
        texteCodePotion.text = "";
        nbRouges = 4;
        nbVertes = 4;
        MettreAJourInterface();
        // On remet les boutons cliquables
        GetComponentInChildren<Canvas>().GetComponentInChildren<GraphicRaycaster>().enabled = true;
    }

    void MettreAJourInterface()
    {
        texteCompteurRouges.text = "Rouges : " + nbRouges;
        texteCompteurVertes.text = "Vertes : " + nbVertes;
    }
}