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
    public float distanceSonPas = 8f;
    public float intervalPas = 0.5f;

    [Header("Alarme Lumières")]
    public Light[] alarmLights;
    public float blinkSpeed = 8f;
    public float minIntensity = 0f;
    public float maxIntensity = 6f;
    public float retourDouxVitesse = 2f; // 🔥 vitesse du retour smooth

    private NavMeshAgent agent;
    private AudioSource audioSource;

    private bool enChasse;
    private bool alarmeActive;
    private float footstepTimer;

    private Color[] originalColors;
    private float[] originalIntensities;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        agent.speed = vitessePatrouille;
        ConfigurerAudio();
        ChoisirNouveauPoint();

        originalColors = new Color[alarmLights.Length];
        originalIntensities = new float[alarmLights.Length];

        for (int i = 0; i < alarmLights.Length; i++)
        {
            originalColors[i] = alarmLights[i].color;
            originalIntensities[i] = alarmLights[i].intensity;
        }
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
                ActiverAlarme();
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
                DesactiverAlarme();
                ChoisirNouveauPoint();
            }

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                ChoisirNouveauPoint();
        }

        GererPas();
        GererLumieres();
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

    void ActiverAlarme()
    {
        alarmeActive = true;

        if (sonAlerte != null)
        {
            audioSource.clip = sonAlerte;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void DesactiverAlarme()
    {
        alarmeActive = false;
        audioSource.Stop();
    }

    void GererLumieres()
    {
        for (int i = 0; i < alarmLights.Length; i++)
        {
            if (alarmeActive)
            {
                float intensity = Mathf.Lerp(
                    minIntensity,
                    maxIntensity,
                    Mathf.PingPong(Time.time * blinkSpeed, 1)
                );

                alarmLights[i].color = Color.red;
                alarmLights[i].intensity = intensity;
            }
            else
            {
                // 🔥 RETOUR DOUX
                alarmLights[i].color = Color.Lerp(
                    alarmLights[i].color,
                    originalColors[i],
                    Time.deltaTime * retourDouxVitesse
                );

                alarmLights[i].intensity = Mathf.Lerp(
                    alarmLights[i].intensity,
                    originalIntensities[i],
                    Time.deltaTime * retourDouxVitesse
                );
            }
        }
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

        if (distance > distanceSonPas)
        {
            footstepTimer = 0f;
            return;
        }

        if (agent.velocity.magnitude < 0.2f)
            return;

        footstepTimer += Time.deltaTime;

        if (footstepTimer >= intervalPas)
        {
            if (sonPas != null)
            {
                float volume = 1f - (distance / distanceSonPas);
                volume = Mathf.Clamp01(volume);
                audioSource.PlayOneShot(sonPas, volume);
            }

            footstepTimer = 0f;
        }
    }

    void ConfigurerAudio()
    {
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 1.5f;
        audioSource.maxDistance = distanceSonPas;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.dopplerLevel = 0f;
        audioSource.playOnAwake = false;
    }
}