using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OuverturePorteSocket : MonoBehaviour
{
    [Header("Configuration Audio")]
    public AudioClip sonOuverture;
    private AudioSource audioSource;

    private XRSocketInteractor socket;
    private bool estDejaOuvert = false;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnEnable() => socket.selectEntered.AddListener(OnKeyInserted);
    private void OnDisable() => socket.selectEntered.RemoveListener(OnKeyInserted);

    private void OnKeyInserted(SelectEnterEventArgs args)
    {
        GameObject objetInsere = args.interactableObject.transform.gameObject;

        if (objetInsere.CompareTag("key") && !estDejaOuvert)
        {
            // On cherche l'objet avec le tag "doorkey" (vu sur ton image_4aab97.png)
            GameObject porte = GameObject.FindWithTag("doorkey");

            if (porte != null)
            {
                estDejaOuvert = true;

                // 1. Son du socle
                if (sonOuverture != null) audioSource.PlayOneShot(sonOuverture);

                // 2. Lancer l'animation par son NOM
                Animator anim = porte.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.enabled = true;
                    // On joue directement le nom du clip d'animation
                    anim.Play("Door_fin");
                }

                // 3. Son de la porte
                AudioSource audioPorte = porte.GetComponent<AudioSource>();
                if (audioPorte != null) audioPorte.Play();

                Destroy(objetInsere, 0.5f);
            }
        }
    }
}