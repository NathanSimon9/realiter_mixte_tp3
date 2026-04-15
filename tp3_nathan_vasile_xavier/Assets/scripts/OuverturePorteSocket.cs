using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OuverturePorteSocket : MonoBehaviour
{
    [Header("Configuration de la Porte")]
    public Animator porteAnimator;
    public string triggerOuverture = "Porte_clee";

    private XRSocketInteractor socket;
    private bool estDejaOuvert = false; // Pour savoir si on a déjà activé la porte

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();

        // Debug pour vérifier si l'Animator est bien lié
        if (porteAnimator == null)
            Debug.LogError("ATTENTION : L'Animator de la porte n'est pas glissé dans le script sur le Socle !");
        else
            Debug.Log("Porte initialisée : Fermée.");
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
                Debug.Log("CLE DETECTEE : Lancement de l'animation Porte_clee.");

                estDejaOuvert = true; // On marque comme ouvert

                if (porteAnimator != null)
                {
                    porteAnimator.SetTrigger(triggerOuverture);
                }

                Destroy(objetInsere, 0.5f);
            }
            else
            {
                Debug.Log("La porte est déjà ouverte, on ignore la clé.");
            }
        }
        else
        {
            Debug.Log("Objet inséré n'a pas le tag 'key'. Tag actuel : " + objetInsere.tag);
        }
    }
}