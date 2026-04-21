using UnityEngine;

public class LevierOneShot : MonoBehaviour
{
    public Animator levierAnimator;
    private bool estFini = false;

    private void OnTriggerEnter(Collider other)
    {
        // Si c'est déjà fait, on sort tout de suite
        if (estFini) return;

        // On lance l'animation
        if (levierAnimator != null)
        {
            estFini = true;
            levierAnimator.Play("levier_01", 0, 0f);
            Debug.Log("<color=green>[OK]</color> Animation lancée une seule fois.");

            // On désactive le Collider pour que plus rien ne puisse le toucher
            BoxCollider bc = GetComponent<BoxCollider>();
            if (bc != null)
            {
                bc.enabled = false;
                Debug.Log("<color=red>[OFF]</color> Zone de détection désactivée.");
            }
        }
    }
}