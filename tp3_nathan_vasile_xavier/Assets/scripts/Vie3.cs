using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GestionVieUI : MonoBehaviour
{
    [Header("Configuration des Cœurs")]
    public List<Image> heartImages; // Glisse tes 3 images du Canvas ici
    public Sprite fullHeart;        // Ton sprite de cœur rouge (créé sur Illustrator/Photoshop)

    void Start()
    {
        // On s'assure que les 3 cœurs sont rouges au départ
        AfficherCoeursPleins();
    }

    public void AfficherCoeursPleins()
    {
        foreach (Image img in heartImages)
        {
            if (img != null)
            {
                img.sprite = fullHeart;
            }
        }
    }
}