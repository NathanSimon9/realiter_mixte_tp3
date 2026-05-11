using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Configuration de la porte")]
    [Tooltip("Cochez la scène correspondante à cette porte dans l'inspecteur")]
    public int sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        // On vérifie si l'objet qui entre dans la porte est le joueur
        if (other.CompareTag("Player"))
        {
            Debug.Log("Le Joueur a franchi la porte. Chargement de la scène : " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}