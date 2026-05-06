using UnityEngine;
using UnityEngine.SceneManagement;

public class VRLavaDeath : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathSound;

    private bool isDying = false; // Ajout d'une sécurité pour ne pas trigger 50 fois

    private void OnTriggerEnter(Collider other)
    {
        if (isDying) return;

        // Debug pour voir quel objet touche la lave
        Debug.Log("Lave touchée par : " + other.name + " | Tag: " + other.tag);

        // Si le Player touche la lave
        if (other.CompareTag("Player"))
        {
            isDying = true;
            Debug.Log("🔥 Mort par la lave ! Lancement du son et reload...");

            if (audioSource != null && deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }
            else
            {
                Debug.LogWarning("🔊 AudioSource ou AudioClip manquant sur la Lave !");
            }

            // On recharge la scène après 0.5s
            Invoke("ReloadScene", 0.5f);
        }
    }

    void ReloadScene()
    {
        Debug.Log("🔄 Reload de la scène suite à une chute dans la lave.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}