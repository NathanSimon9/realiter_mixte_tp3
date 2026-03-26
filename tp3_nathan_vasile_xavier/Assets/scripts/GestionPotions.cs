using UnityEngine;
using TMPro; // Obligatoire pour utiliser TextMeshPro
using UnityEngine.UI; // Obligatoire pour interagir avec les Boutons

public class GestionPotions : MonoBehaviour
{
    [Header("--- Configuration des Textes (UI) ---")]
    [Tooltip("Glisse l'objet 'Codepotion' ici")]
    public TextMeshProUGUI texteCodePotion;

    [Tooltip("Glisse l'objet 'TexteRougesPossedees' ici")]
    public TextMeshProUGUI texteCompteurRouges;

    [Tooltip("Glisse l'objet 'TexteVertesPossedees' ici")]
    public TextMeshProUGUI texteCompteurVertes;

    [Header("--- Configuration des Boutons (UI) ---")]
    [Tooltip("Glisse l'objet 'Redbutton' ici")]
    public Button boutonRouge;

    [Tooltip("Glisse l'objet 'GreenButton' ici")]
    public Button boutonVert;

    [Header("--- Inventaire de départ ---")]
    // Comme on vient de changer de scène, on redonne manuellement les 4 bouteilles
    private int nbRouges = 4;
    private int nbVertes = 4;

    void Start()
    {
        // 1. Initialisation : On s'assure que le texte du code est vide au début
        if (texteCodePotion != null)
            texteCodePotion.text = "";

        // 2. Initialisation : On affiche '4' dans les compteurs
        MettreAJourInterface();
    }

    // --- FONCTION POUR LE BOUTON ROUGE (Redbutton) ---
    // Cette fonction doit être liée à l'événement OnClick du Redbutton dans l'inspecteur
    public void ActionBoutonRouge()
    {
        // On vérifie s'il reste des bouteilles rouges
        if (nbRouges > 0)
        {
            // 1. Diminue le stock
            nbRouges--;

            // 2. Ajoute 'R' à la fin du texte existant (de gauche à droite)
            texteCodePotion.text = texteCodePotion.text + "R";

            // 3. Met à jour les textes affichés
            MettreAJourInterface();

            // Debug pour vérifier dans la console
            Debug.Log("Rouge ajoutée. Code actuel : " + texteCodePotion.text);
        }
        else
        {
            Debug.Log("Plus de bouteilles rouges disponibles !");
            // Optionnel : jouer un son d'erreur ici
        }
    }

    // --- FONCTION POUR LE BOUTON VERT (GreenButton) ---
    // Cette fonction doit être liée à l'événement OnClick du GreenButton dans l'inspecteur
    public void ActionBoutonVert()
    {
        // On vérifie s'il reste des bouteilles vertes
        if (nbVertes > 0)
        {
            // 1. Diminue le stock
            nbVertes--;

            // 2. Ajoute 'V' à la fin du texte existant (de gauche à droite)
            texteCodePotion.text = texteCodePotion.text + "V";

            // 3. Met à jour les textes affichés
            MettreAJourInterface();

            // Debug pour vérifier dans la console
            Debug.Log("Verte ajoutée. Code actuel : " + texteCodePotion.text);
        }
        else
        {
            Debug.Log("Plus de bouteilles vertes disponibles !");
        }
    }

    // Fonction outil pour rafraîchir tous les textes de l'interface
    void MettreAJourInterface()
    {
        // Mise à jour des compteurs numériques
        if (texteCompteurRouges != null)
            texteCompteurRouges.text = "Rouges : " + nbRouges;

        if (texteCompteurVertes != null)
            texteCompteurVertes.text = "Vertes : " + nbVertes;

        // --- GESTION DE L'INTERACTIVITÉ DES BOUTONS ---
        // Si le stock tombe à 0, on rend le bouton gris et non-cliquable
        if (nbRouges <= 0 && boutonRouge != null)
            boutonRouge.interactable = false;

        if (nbVertes <= 0 && boutonVert != null)
            boutonVert.interactable = false;
    }
}