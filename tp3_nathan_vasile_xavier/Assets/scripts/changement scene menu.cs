using UnityEngine;
using UnityEngine.SceneManagement; // Obligatoire pour changer de scène

public class GestionnaireScene : MonoBehaviour
{
    // Cette fonction sera appelée par le bouton
    public void LancerSceneUn()
    {
        // Charge la scène à l'index 1 (dépend de tes Build Settings)
        SceneManager.LoadScene(1);
    }
}