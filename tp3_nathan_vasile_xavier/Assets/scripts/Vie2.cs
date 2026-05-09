using UnityEngine;
using System.Collections.Generic;

public class Vie2 : MonoBehaviour
{
    [Header("Tes 2 Objets Coeurs dans le Canvas")]
    public List<GameObject> heartObjects;

    void Start()
    {
        // On s'assure que tes 2 cœurs sont visibles au début du niveau
        foreach (GameObject heart in heartObjects)
        {
            if (heart != null) heart.SetActive(true);
        }
    }
}