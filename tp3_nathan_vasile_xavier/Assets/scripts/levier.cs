using UnityEngine;

public class LevierExpertDebug : MonoBehaviour
{
    [Header("--- LEVIER (L'objet actuel) ---")]
    public AnimationClip animationDuLevier;

    [Header("--- TRAPPE (L'objet au sol) ---")]
    public Animator scriptAnimatorDeLaTrappe;
    public AnimationClip animationDeLaTrappe;

    [Header("--- PORTE SUPPLEMENTAIRE ---")]
    public Animator scriptAnimatorDeLaPorte;
    public AnimationClip animationDeLaPorte;

    private bool dejaActive = false;

    private void OnTriggerEnter(Collider other)
    {
        // On vérifie que c'est le joueur et qu'on n'a pas déjà activé le levier
        if (other.CompareTag("Player") && !dejaActive)
        {
            dejaActive = true;

            // 1. JOUE L'ANIMATION DU LEVIER
            Animator animLevier = GetComponent<Animator>();
            if (animLevier != null && animationDuLevier != null)
            {
                animLevier.Play(animationDuLevier.name, 0, 0f);
                Debug.Log("<color=lime><b>[LEVIER]</b></color> Animation lancée !");
            }

            // 2. JOUE L'ANIMATION DE LA TRAPPE
            if (scriptAnimatorDeLaTrappe != null && animationDeLaTrappe != null)
            {
                scriptAnimatorDeLaTrappe.Play(animationDeLaTrappe.name, 0, 0f);
                Debug.Log("<color=cyan><b>[TRAPPE]</b></color> Animation " + animationDeLaTrappe.name + " lancée !");
            }

            // 3. JOUE L'ANIMATION DE LA PORTE
            if (scriptAnimatorDeLaPorte != null && animationDeLaPorte != null)
            {
                scriptAnimatorDeLaPorte.Play(animationDeLaPorte.name, 0, 0f);
                Debug.Log("<color=orange><b>[PORTE]</b></color> Animation " + animationDeLaPorte.name + " lancée !");
            }

            // 4. ON DÉSACTIVE TOUT POUR NE PAS RECOMMENCER
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            Destroy(this); // Détruit le script (le levier reste là mais ne réagit plus)
        }
    }
}