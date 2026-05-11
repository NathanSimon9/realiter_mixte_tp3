using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class EnemyVision360 : MonoBehaviour
{
    [Header("Références")]
    public Transform joueur;
    public LayerMask obstacleMask;

    [Header("Réglages Vision/Vitesse")]
    public float distanceVision = 10f;
    public float vitessePatrouille = 2f;
    public float vitesseChasse = 6f;
    public float rayonPatrouille = 15f;

    [Header("Sons de Pas (3D)")]
    public AudioClip sonPas;
    public float distanceSonPas = 8f;
    public float intervalPas = 0.5f;
    [Range(0f, 1f)] public float volumePasMax = 0.4f;

    private NavMeshAgent agent;
    private AudioSource audioSource;
    private bool enChasse = false;
    private float footstepTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        agent.speed = vitessePatrouille;
        ConfigurerAudioLocal();
        ChoisirNouveauPoint();
    }

    void Update()
    {
        if (joueur == null) return;

        if (PeutVoirJoueur())
        {
            if (!enChasse) ActiverModeChasse();

            agent.SetDestination(joueur.position);
        }
        else
        {
            if (enChasse) DesactiverModeChasse();

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                ChoisirNouveauPoint();
        }

        GererPas();
    }

    bool PeutVoirJoueur()
    {
        float dist = Vector3.Distance(transform.position, joueur.position);
        if (dist > distanceVision) return false;

        Vector3 dir = (joueur.position - transform.position).normalized;
        return !Physics.Raycast(transform.position + Vector3.up, dir, dist, obstacleMask);
    }

    void ActiverModeChasse()
    {
        enChasse = true;
        agent.speed = vitesseChasse;
        if (AlarmLightManager.Instance != null)
            AlarmLightManager.Instance.StartAlarm();
    }

    void DesactiverModeChasse()
    {
        enChasse = false;
        agent.speed = vitessePatrouille;
        if (AlarmLightManager.Instance != null)
            AlarmLightManager.Instance.StopAlarm();

        ChoisirNouveauPoint();
    }

    void ChoisirNouveauPoint()
    {
        Vector3 randDir = Random.insideUnitSphere * rayonPatrouille + transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randDir, out hit, rayonPatrouille, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }

    void GererPas()
    {
        if (agent.velocity.magnitude < 0.2f) return;

        float dist = Vector3.Distance(transform.position, joueur.position);
        if (dist > distanceSonPas) return;

        footstepTimer += Time.deltaTime;
        if (footstepTimer >= intervalPas)
        {
            float vol = (1f - (dist / distanceSonPas)) * volumePasMax;
            audioSource.PlayOneShot(sonPas, Mathf.Clamp01(vol));
            footstepTimer = 0f;
        }
    }

    void ConfigurerAudioLocal()
    {
        audioSource.spatialBlend = 1f; // 3D pour les pas
        audioSource.playOnAwake = false;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = distanceSonPas;
    }
}