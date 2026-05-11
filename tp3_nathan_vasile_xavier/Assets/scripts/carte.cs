using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this script to your trigger box (IsTrigger = true).
// Assign the map Canvas GameObject in the inspector ("Carte Canvas").
// If the Canvas is inactive at Start, assign it manually in the inspector (GameObject.FindWithTag cannot find inactive objects).
public class carte : MonoBehaviour
{
    [Header("Carte (Canvas)")]
    [Tooltip("Drag your Canvas GameObject here. If null, the script will try to find an active object with tag 'carte'.")]
    [SerializeField] private GameObject carteCanvas;

    [Header("Sons")]
    [Tooltip("Son joué lorsque la carte apparaît")]
    [SerializeField] private AudioClip sonCarte;
    [Range(0f, 1f)]
    [SerializeField] private float sonCarteVolume = 1f;

    [Header("Tags & options")]
    [SerializeField] private string carteTag = "carte"; // used only if carteCanvas is not assigned
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool hideOnStart = true;
    [SerializeField] private bool openOnEnter = true;
    [SerializeField] private bool closeOnExit = true;

    private void Start()
    {
        if (carteCanvas == null && !string.IsNullOrEmpty(carteTag))
        {
            try
            {
                GameObject found = GameObject.FindWithTag(carteTag);
                if (found != null)
                    carteCanvas = found;
            }
            catch (UnityException)
            {
                // tag not defined - ignore
            }
        }

        if (hideOnStart && carteCanvas != null)
            carteCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!openOnEnter) return;
        if (IsPlayer(other))
        {
            if (carteCanvas != null)
            {
                bool wasActive = carteCanvas.activeSelf;
                // n'ouvre et ne joue le son que si la carte était fermée
                if (!wasActive)
                {
                    carteCanvas.SetActive(true);
                    PlayCarteSoundAtCollider(other);
                }
            }
            else
                Debug.LogWarning("[carte] carteCanvas is null. Assign it in the Inspector or ensure an active object has the tag '" + carteTag + "'.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!closeOnExit) return;
        if (IsPlayer(other))
        {
            if (carteCanvas != null)
                carteCanvas.SetActive(false);
        }
    }

    private bool IsPlayer(Collider other)
    {
        if (other == null) return false;
        if (other.CompareTag(playerTag)) return true;
        if (other.transform.root != null && other.transform.root.CompareTag(playerTag)) return true;
        // fallback: accept MainCamera if used as player collider
        if (other.CompareTag("MainCamera") || (other.transform.root != null && other.transform.root.CompareTag("MainCamera"))) return true;
        return false;
    }

    private void PlayCarteSoundAtCollider(Collider other)
    {
        if (sonCarte == null) return;

        Vector3 pos = transform.position;
        if (other != null)
        {
            pos = other.transform.position;
        }
        else if (Camera.main != null)
        {
            pos = Camera.main.transform.position;
        }

        AudioSource.PlayClipAtPoint(sonCarte, pos, sonCarteVolume);
    }
}
