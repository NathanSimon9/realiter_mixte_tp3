using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Si on touche la première sortie
        if (other.CompareTag("exit1"))
        {
            SceneManager.LoadScene(2);
        }
        // Si on touche la deuxième sortie
        if (other.CompareTag("exit2"))
        {
            SceneManager.LoadScene(3);
        }
    }
}