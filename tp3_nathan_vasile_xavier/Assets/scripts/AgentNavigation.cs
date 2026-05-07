using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentNavigation : MonoBehaviour
{
    public GameObject objetCible;
    
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating("RecalculerRoute", 1f, 2f);
    }

    void RecalculerRoute()
    {
        agent.SetDestination(objetCible.transform.position);
    }
}
