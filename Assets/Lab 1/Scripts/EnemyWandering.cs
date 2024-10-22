using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWandering : MonoBehaviour 
{
    public NavMeshAgent agent;
    public float rangesphere = 10;
    public Transform centrePoint; 
    public Transform player;
    public float detectionRadius = 10f;
    public float fovAngle = 60f;
    public LayerMask obstacleMask;
    private Camera tempCamera;
    private bool playerDetected = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Crear cámara temporal simulada
        GameObject camObj = new GameObject("TempCam");
        tempCamera = camObj.AddComponent<Camera>();
        tempCamera.enabled = false;
        centrePoint = GameObject.Find("CenterPoint").transform;
        player = GameObject.Find("PlayerLab3").transform;
    }

    void Update()
    {
        if (PlayerInFOV())
        {
            // Si se detecta al jugador, enviar la posición a todos los zombies usando BroadcastMessage
            BroadcastMessage("OnPlayerDetected", player.position, SendMessageOptions.DontRequireReceiver);
            FollowPlayer();
        }
        else if (!playerDetected && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Movimiento aleatorio si no hay jugador detectado
            Vector3 point;
            if (RandomPoint(centrePoint.position, rangesphere, out point)) 
            {
                agent.SetDestination(point);
            }
        }

    }

    bool PlayerInFOV()
    {
        // Configurar cámara simulada para calcular frustum
        tempCamera.transform.position = transform.position;
        tempCamera.transform.rotation = transform.rotation;
        tempCamera.fieldOfView = fovAngle;
        tempCamera.aspect = 1.0f;
        tempCamera.nearClipPlane = 0.01f;
        tempCamera.farClipPlane = detectionRadius;

        Vector3[] frustumCorners = new Vector3[4];
        tempCamera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), detectionRadius, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);

        // Verificar si el jugador está en el frustum y no hay obstáculos
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer <= fovAngle / 2)
            {
                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask))
                {
                    return true;
                }
            }
        }

        return false;
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
        playerDetected = true;
        agent.destination = player.position;
    }

    void OnPlayerDetected(Vector3 playerPosition)
    {
        if(playerDetected)
            return; 
        Debug.Log("Player detected by another enemy");
        agent.destination = playerPosition;
        playerDetected = true;
    }

    void OnDrawGizmos()
    {
        if (centrePoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(centrePoint.position, rangesphere);
        }

        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            if (tempCamera != null)
            {
                Vector3[] frustumCorners = new Vector3[4];
                tempCamera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), detectionRadius, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);

                for (int i = 0; i < frustumCorners.Length; i++)
                {
                    frustumCorners[i] = tempCamera.transform.TransformPoint(frustumCorners[i]);
                }

                Gizmos.DrawLine(transform.position, frustumCorners[0]);
                Gizmos.DrawLine(transform.position, frustumCorners[1]);
                Gizmos.DrawLine(transform.position, frustumCorners[2]);
                Gizmos.DrawLine(transform.position, frustumCorners[3]);

                Gizmos.DrawLine(frustumCorners[0], frustumCorners[1]);
                Gizmos.DrawLine(frustumCorners[1], frustumCorners[2]);
                Gizmos.DrawLine(frustumCorners[2], frustumCorners[3]);
                Gizmos.DrawLine(frustumCorners[3], frustumCorners[0]);
            }
        }
    }
}
