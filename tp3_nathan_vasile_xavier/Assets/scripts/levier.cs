using UnityEngine;

public class LevierExpertDebug : MonoBehaviour
{
    [Header("Drag & Drop l'animation bleue ici")]
    public AnimationClip clipAxe;

    private bool dejaActive = false;

    private void Awake()
    {
        // DEBUG AU CHARGEMENT
        Debug.Log("<color=magenta><b>[SYSTEM]</b></color> Levier prêt. J'attends le tag 'Player'.");

        Animator anim = GetComponent<Animator>();
        if (anim == null) Debug.LogError("<color=red><b>[ERREUR]</b></color> Aucun Animator trouvé sur ce levier !");
        if (clipAxe == null) Debug.LogWarning("<color=yellow><b>[WARNING]</b></color> Tu as oublié de glisser l'animation dans la case !");
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. DÉTECTION BRUTE
        Debug.Log("<color=white><b>[COLLISION]</b></color> Objet : <b>" + other.name + "</b> | Tag : <b>" + other.tag + "</b>");

        // 2. FILTRE JOUEUR
        if (other.CompareTag("Player"))
        {
            Debug.Log("<color=cyan><b>[VERIFICATION]</b></color> Le Tag est correct. Tentative d'activation...");

            if (!dejaActive)
            {
                Animator anim = GetComponent<Animator>();
                if (anim != null && clipAxe != null)
                {
                    dejaActive = true;

                    // 3. LANCEMENT ANIMATION
                    anim.Play(clipAxe.name, 0, 0f);
                    Debug.Log("<color=lime><b>[ACTION]</b></color> Animation <b>" + clipAxe.name + "</b> lancée avec succès !");

                    // 4. NETTOYAGE PHYSIQUE
                    Collider col = GetComponent<Collider>();
                    if (col != null)
                    {
                        col.enabled = false;
                        Debug.Log("<color=orange><b>[PHYSIQUE]</b></color> BoxCollider désactivé pour éviter les doublons.");
                    }

                    // 5. AUTODESTRUCTION DU SCRIPT
                    Debug.Log("<color=red><b>[CLEANUP]</b></color> Suppression du script. Mission terminée.");
                    Destroy(this);
                }
            }
            else
            {
                Debug.Log("<color=gray><b>[REFUS]</b></color> Déjà activé, j'ignore le contact.");
            }
        }
        else
        {
            // SI UN OBJET SANS LE TAG PLAYER TOUCHE LE LEVIER
            Debug.Log("<color=yellow><b>[INTRUS]</b></color> Contact avec <b>" + other.name + "</b> ignoré car le tag n'est pas 'Player'.");
        }
    }
}