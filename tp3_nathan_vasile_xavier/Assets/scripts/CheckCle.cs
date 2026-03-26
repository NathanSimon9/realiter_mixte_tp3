using UnityEngine;

public class CheckCle : MonoBehaviour
{
      public GameObject visuelCle;

    private Animator anim;

    void Start()
    {
        // On récupère l'Animator automatiquement sur l'objet Clee
        anim = GetComponentInChildren<Animator>();

        // 1. On cache la pièce au début
        if (visuelCle != null)
        {
            visuelCle.SetActive(false);
        }

        // 2. Si le mini-jeu est réussi, on l'affiche et on anime
        if (GestionDonnees.codeCouleurReussi == true)
        {
            if (visuelCle != null) visuelCle.SetActive(true);

            if (anim != null)
            {
                anim.Play("clee"); // Vérifie bien ce nom dans ton Animator
                Debug.Log("REUSSI : Animation de la clé lancée.");
            }
        }
    }
}