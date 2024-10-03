using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehavior : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform player;
    public float detectionRadius = 10f;
    private int currentPointIndex = 0;
    [SerializeField] private bool isPositiveDirection;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentPointIndex = Random.Range(0, patrolPoints.Length);
        isPositiveDirection = Random.Range(0, 2) == 0;
    }
    
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            FollowPlayer();
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }
    }

    void FollowPlayer()
    {
        agent.destination = player.position;
    }

    void GotoNextPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPointIndex].position;
        if(isPositiveDirection)
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        else 
            currentPointIndex = (currentPointIndex - 1 + patrolPoints.Length) % patrolPoints.Length;
    }
}