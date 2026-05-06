using UnityEngine;
using UnityEngine.SceneManagement;

public class VRTrapDeath : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathSound;

    private bool isDying = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isDying) return;

        // Debug pour voir ce qui touche le trigger
        Debug.Log("Trigger touché par : " + other.name + " (Tag: " + other.tag + ")");

        if (other.CompareTag("Player"))
        {
            CheckTagAndDie();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDying) return;

        // Debug pour voir ce qui entre en collision physique
        Debug.Log("Collision avec : " + collision.gameObject.name + " (Tag: " + collision.gameObject.tag + ")");

        if (collision.gameObject.CompareTag("Player"))
        {
            CheckTagAndDie();
        }
    }

    void CheckTagAndDie()
    {
        // Debug pour vérifier le tag de l'objet qui porte ce script
        Debug.Log("Vérification du tag du piège : " + gameObject.tag);

        if (gameObject.CompareTag("sol_spike") ||
            gameObject.CompareTag("scie") ||
            gameObject.CompareTag("grinder_triple") ||
            gameObject.CompareTag("axe_swing"))
        {
            Die();
        }
        else
        {
            Debug.LogWarning("⚠️ Le joueur a touché l'objet, mais le tag '" + gameObject.tag + "' n'est pas reconnu dans le script de mort.");
        }
    }

    void Die()
    {
        isDying = true;
        Debug.Log("💀 Mort enclenchée ! Son de mort lancé.");

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        else
        {
            Debug.LogWarning("🔊 AudioSource ou AudioClip manquant sur " + gameObject.name);
        }

        Invoke("ReloadScene", 0.5f);
    }

    void ReloadScene()
    {
        Debug.Log("🔄 Rechargement de la scène : " + SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}