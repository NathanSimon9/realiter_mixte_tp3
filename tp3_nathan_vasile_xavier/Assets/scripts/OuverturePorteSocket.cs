using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OuverturePorteSocket : MonoBehaviour
{
    [Header("Configuration de la Porte")]
    public Animator porteAnimator;
    public string triggerOuverture = "Porte_clee";

    [Header("Configuration Audio")]
    public AudioClip sonOuverture; // Glisse ton son "aparitions" ici
    private AudioSource audioSource;

    private XRSocketInteractor socket;
    private bool estDejaOuvert = false;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();

        // On prépare l'AudioSource sur le Socle
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (porteAnimator == null)
        {
            Debug.LogError("ATTENTION : L'objet 'Door_fin' n'est pas assigné !");
        }
    }

    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnKeyInserted);
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnKeyInserted);
    }

    private void OnKeyInserted(SelectEnterEventArgs args)
    {
        GameObject objetInsere = args.interactableObject.transform.gameObject;

        if (objetInsere.CompareTag("key"))
        {
            if (!estDejaOuvert)
            {
                Debug.Log("CLE DETECTEE : Ouverture de Door_fin.");
                estDejaOuvert = true;

                // 1. Son du Socle (le "clic" de la clé)
                if (sonOuverture != null) audioSource.PlayOneShot(sonOuverture);

                if (porteAnimator != null)
                {
                    // 2. Lancement de l'animation
                    porteAnimator.SetTrigger(triggerOuverture);

                    // 3. ON JOUE LE SON SUR LA PORTE ICI
                    AudioSource audioPorte = porteAnimator.GetComponent<AudioSource>();
                    if (audioPorte != null)
                    {
                        audioPorte.Play();
                    }
                }

                Destroy(objetInsere, 0.5f);
            }
        }
    }
}