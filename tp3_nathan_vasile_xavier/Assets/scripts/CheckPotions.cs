using UnityEngine;

public class CheckPotions : MonoBehaviour
{
    void Start()
    {
        // Si la boîte aux lettres dit "true", on détruit la potion
        if (GestionDonnees.potionsRecoltees == true)
        {
            Destroy(gameObject);
        }
    }
}