using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OuverturePorteSocket : MonoBehaviour
{
    [Header("Configuration de la Porte")]
    [Tooltip("Glisse ici l'objet nommé Door_fin")]
    public Animator porteAnimator;
    public string triggerOuverture = "Porte_clee";

    private XRSocketInteractor socket;
    private bool estDejaOuvert = false;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();

        // Vérification automatique au démarrage
        if (porteAnimator == null)
        {
            Debug.LogError("ATTENTION : L'objet 'Door_fin' (Animator) n'est pas assigné dans le script sur le Socle !");
        }
        else if (porteAnimator.gameObject.name != "Door_fin")
        {
            Debug.LogWarning("Note : L'objet assigné ne s'appelle pas 'Door_fin', mais il fonctionnera quand même s'il a l'Animator.");
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

        // On garde ta logique de tag "key" pour l'objet qu'on tient en main
        if (objetInsere.CompareTag("key"))
        {
            if (!estDejaOuvert)
            {
                Debug.Log("CLE DETECTEE : Ouverture de Door_fin.");

                estDejaOuvert = true;

                if (porteAnimator != null)
                {
                    porteAnimator.SetTrigger(triggerOuverture);
                }

                Destroy(objetInsere, 0.5f);
            }
            else
            {
                Debug.Log("Door_fin est déjà ouverte.");
            }
        }
        else
        {
            Debug.Log("L'objet inséré n'est pas une clé (Tag 'key' manquant).");
        }
    }
}