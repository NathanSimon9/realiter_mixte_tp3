using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class EnemyVision360 : MonoBehaviour
{
    [Header("Référence joueur (Main Camera VR)")]
    public Transform joueur;

    [Header("Vision")]
    public float distanceVision = 10f;
    public LayerMask obstacleMask;

    [Header("Vitesses")]
    public float vitessePatrouille = 2f;
    public float vitesseChasse = 6f;

    [Header("Patrouille")]
    public float rayonPatrouille = 15f;

    [Header("Sons")]
    public AudioClip sonAlerte;
    public AudioClip sonPas;

    [Header("Audio Distance")]
    public float distanceSonPas = 8f;   // Distance max pour entendre les pas
    public float intervalPas = 0.5f;

    private NavMeshAgent agent;
    private AudioSource audioSource;

    private bool enChasse;
    private float footstepTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        agent.speed = vitessePatrouille;
        ConfigurerAudio();
        ChoisirNouveauPoint();
    }

    void Update()
    {
        if (joueur == null) return;

        if (PeutVoirJoueur())
        {
            if (!enChasse)
            {
                enChasse = true;
                agent.speed = vitesseChasse;

                if (sonAlerte != null)
                    audioSource.PlayOneShot(sonAlerte);
            }

            Vector3 targetPos = joueur.position;
            targetPos.y = transform.position.y;

            agent.SetDestination(targetPos);
        }
        else
        {
            if (enChasse)
            {
                enChasse = false;
                agent.speed = vitessePatrouille;
                ChoisirNouveauPoint();
            }

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                ChoisirNouveauPoint();
            }
        }

        GererPas();
    }

    bool PeutVoirJoueur()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, joueur.position);

        if (distanceToPlayer > distanceVision)
            return false;

        Vector3 direction = (joueur.position - transform.position).normalized;

        if (Physics.Raycast(transform.position + Vector3.up, direction, distanceToPlayer, obstacleMask))
            return false;

        return true;
    }

    void ChoisirNouveauPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * rayonPatrouille;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, rayonPatrouille, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void GererPas()
    {
        if (joueur == null) return;

        float distance = Vector3.Distance(transform.position, joueur.position);

        // Trop loin → on ne joue rien
        if (distance > distanceSonPas)
        {
            footstepTimer = 0f;
            return;
        }

        // Pas en mouvement → rien
        if (agent.velocity.magnitude < 0.2f)
            return;

        footstepTimer += Time.deltaTime;

        if (footstepTimer >= intervalPas)
        {
            if (sonPas != null)
            {
                // Volume augmente quand il se rapproche
                float volume = 1f - (distance / distanceSonPas);
                volume = Mathf.Clamp01(volume);

                audioSource.PlayOneShot(sonPas, volume);
            }

            footstepTimer = 0f;
        }
    }

    void ConfigurerAudio()
    {
        audioSource.spatialBlend = 1f;      // Full 3D
        audioSource.minDistance = 1.5f;
        audioSource.maxDistance = distanceSonPas;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.dopplerLevel = 0f;
        audioSource.playOnAwake = false;
    }
}