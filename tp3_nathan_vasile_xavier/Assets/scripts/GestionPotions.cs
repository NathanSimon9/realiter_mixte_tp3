using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GestionPotions : MonoBehaviour
{
    [Header("--- UI Textes ---")]
    public TextMeshProUGUI texteCodePotion;
    public TextMeshProUGUI texteCompteurRouges;
    public TextMeshProUGUI texteCompteurVertes;

    [Header("--- Messages de Fin (GameObjects) ---")]
    public GameObject textereussite;
    public GameObject texteechec;

    [Header("--- Boutons ---")]
    public Button boutonRouge;
    public Button boutonVert;

    [Header("--- Configuration ---")]
    public string codeVictoire = "RVRRV";
    public string sceneRetour = "scene_donjon";

    private int nbRouges = 4;
    private int nbVertes = 4;
    private bool enAttente = false;

    void Start()
    {
        Debug.Log("[POTION] Script démarré. Initialisation...");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (textereussite != null)
        {
            textereussite.SetActive(false);
            Debug.Log("[POTION] Texte Réussite trouvé et caché.");
        }
        else Debug.LogError("[POTION] ERREUR : textereussite n'est pas assigné !");

        if (texteechec != null)
        {
            texteechec.SetActive(false);
            Debug.Log("[POTION] Texte Échec trouvé et caché.");
        }
        else Debug.LogError("[POTION] ERREUR : texteechec n'est pas assigné !");

        if (texteCodePotion != null)
        {
            texteCodePotion.text = "";
            texteCodePotion.gameObject.SetActive(true);
        }

        MettreAJourInterface();
    }

    public void ActionBoutonRouge()
    {
        Debug.Log("[POTION] Clic sur Bouton Rouge. Stock : " + nbRouges);
        if (nbRouges > 0 && !enAttente)
        {
            nbRouges--;
            texteCodePotion.text += "R";
            Debug.Log("[POTION] Code actuel : " + texteCodePotion.text);
            MettreAJourInterface();
            VerifierCombinaison();
        }
    }

    public void ActionBoutonVert()
    {
        Debug.Log("[POTION] Clic sur Bouton Vert. Stock : " + nbVertes);
        if (nbVertes > 0 && !enAttente)
        {
            nbVertes--;
            texteCodePotion.text += "V";
            Debug.Log("[POTION] Code actuel : " + texteCodePotion.text);
            MettreAJourInterface();
            VerifierCombinaison();
        }
    }

    void VerifierCombinaison()
    {
        if (texteCodePotion.text == codeVictoire)
        {
            Debug.Log("[POTION] VICTOIRE ! Le code correspond.");
            StartCoroutine(SequenceVictoire());
        }
        else if (texteCodePotion.text.Length >= codeVictoire.Length)
        {
            Debug.Log("[POTION] ÉCHEC ! Code incorrect : " + texteCodePotion.text);
            StartCoroutine(SequenceEchec());
        }
    }

    IEnumerator SequenceVictoire()
    {
        enAttente = true;
        Debug.Log("[POTION] Lancement SequenceVictoire. Attente 2s...");

        if (texteCodePotion != null) texteCodePotion.gameObject.SetActive(false);
        if (textereussite != null) textereussite.SetActive(true);

        yield return new WaitForSeconds(2f);

        Debug.Log("[POTION] Chargement scène : " + sceneRetour);
        SceneManager.LoadScene(sceneRetour);
    }

    IEnumerator SequenceEchec()
    {
        enAttente = true;
        Debug.Log("[POTION] Lancement SequenceEchec. Attente 5s...");

        if (texteCodePotion != null) texteCodePotion.gameObject.SetActive(false);
        if (texteechec != null) texteechec.SetActive(true);

        yield return new WaitForSeconds(5f);

        Debug.Log("[POTION] Retour scène : " + sceneRetour);
        SceneManager.LoadScene(sceneRetour);
    }

    void MettreAJourInterface()
    {
        if (texteCompteurRouges != null) texteCompteurRouges.text = "Rouges : " + nbRouges;
        if (texteCompteurVertes != null) texteCompteurVertes.text = "Vertes : " + nbVertes;

        if (boutonRouge != null) boutonRouge.interactable = (nbRouges > 0 && !enAttente);
        if (boutonVert != null) boutonVert.interactable = (nbVertes > 0 && !enAttente);
    }
}