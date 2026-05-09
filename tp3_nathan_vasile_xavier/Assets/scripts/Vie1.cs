using UnityEngine;
using System.Collections.Generic;

public class Vie1 : MonoBehaviour
{
    [Header("Ton unique objet Coeur")]
    public List<GameObject> heartObjects; // Glisse ton image de coeur ici

    void Start()
    {
        // On s'assure que ton coeur est visible au début
        if (heartObjects.Count > 0 && heartObjects[0] != null)
        {
            heartObjects[0].SetActive(true);
        }
    }
}