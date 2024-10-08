using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyWandering : MonoBehaviour 
{
    public NavMeshAgent agent;
    public float rangesphere;

    public Transform centrePoint; 
    public Transform player;
    public float detectionRadius = 10f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            FollowPlayer();
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, rangesphere, out point)) 
            {
                agent.SetDestination(point);
            }
        }

    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) 
        {
            
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    void FollowPlayer()
    {
        agent.destination = player.position;
    }

    void OnDrawGizmos()
    {
        if (centrePoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(centrePoint.position, rangesphere);
        }
    }

}