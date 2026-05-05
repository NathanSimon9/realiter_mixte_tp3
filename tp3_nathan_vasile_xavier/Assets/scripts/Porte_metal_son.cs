using UnityEngine;

public class Porte_metal_son : MonoBehaviour
{
    public AudioClip cell_door;

    private AudioSource sourcePorte;
    private bool dejaJoue = false;

    void Start()
    {
        sourcePorte = GetComponentInChildren<AudioSource>();

        if (sourcePorte == null)
        {
            Debug.LogError("❌ Aucun AudioSource !");
            return;
        }

        sourcePorte.loop = false;
        sourcePorte.spatialBlend = 1f;

        Debug.Log("✅ Porte prête");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!EstMain(collision.collider) || !EstCellule())
            return;

        Debug.Log("🖐️ Main touche porte");

        if (!dejaJoue)
        {
            PlaySon();
            dejaJoue = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (!EstMain(collision.collider) || !EstCellule())
            return;

        Debug.Log("✋ Main quitte porte");

        // reset pour autoriser un prochain son
        dejaJoue = false;
    }

    void PlaySon()
    {
        if (cell_door == null)
        {
            Debug.LogError("❌ Audio manquant");
            return;
        }

        sourcePorte.clip = cell_door;
        sourcePorte.pitch = Random.Range(0.95f, 1.05f);

        Debug.Log("🎵 PLAY cell_door ONCE");

        sourcePorte.Play();
    }

    bool EstMain(Collider col)
    {
        return col.CompareTag("Player") || col.transform.root.CompareTag("Player");
    }

    bool EstCellule()
    {
        if (!gameObject.tag.StartsWith("cel")) return false;

        int n;
        if (int.TryParse(gameObject.tag.Substring(3), out n))
            return n >= 1 && n <= 6;

        return false;
    }
}