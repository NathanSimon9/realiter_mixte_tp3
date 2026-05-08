using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Référence joueur (Main Camera VR)")]
    public Transform joueur;

    [Header("Vitesses")]
    public float vitessePatrouille = 2f;
    public float vitesseChasse = 5f;

    [Header("Patrouille")]
    public float rayonPatrouille = 15f;
    public float tempsAttente = 2f;

    [Header("Détection")]
    public float distanceDetection = 8f;

    private NavMeshAgent agent;
    private Vector3 pointPatrouille;
    private float timer;
    private bool enChasse;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = vitessePatrouille;
        ChoisirNouveauPoint();
    }

    void Update()
    {
        if (joueur == null) return;

        float distance = Vector3.Distance(transform.position, joueur.position);

        // 🎯 CHASSE
        if (distance <= distanceDetection)
        {
            if (!enChasse)
            {
                enChasse = true;
                agent.speed = vitesseChasse; // 🔥 plus rapide
            }

            Vector3 targetPosition = joueur.position;
            targetPosition.y = transform.position.y;

            agent.SetDestination(targetPosition);
        }
        else
        {
            // 🔁 Retour en patrouille
            if (enChasse)
            {
                enChasse = false;
                agent.speed = vitessePatrouille; // 🚶 plus lent
                ChoisirNouveauPoint();
            }

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                timer += Time.deltaTime;

                if (timer >= tempsAttente)
                {
                    ChoisirNouveauPoint();
                    timer = 0f;
                }
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
            pointPatrouille = hit.position;
            agent.SetDestination(pointPatrouille);
        }
    }
}