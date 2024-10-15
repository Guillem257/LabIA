using UnityEngine;
using System.Collections.Generic;

public class Flock2 : MonoBehaviour
{
    public float speed;
    private bool isLeader = false;
    private int currentPatrolPoint = 0;

    void Start()
    {
        // Inicializa la velocidad del fantasma
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);

        // Determina si este fantasma es un líder
        isLeader = FlockManager.FM.leaders.Contains(gameObject);
    }

    void Update()
    {
        // Comprueba los límites del área de patrullaje
        Bounds b = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.swimLimits * 2);
        if (!b.Contains(transform.position))
        {
            Vector3 dir = FlockManager.FM.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(dir),
                                                  FlockManager.FM.rotationSpeed * Time.deltaTime);
            speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
        }
        else if (Random.Range(0, 100) < 10)
        {
            speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
        }

        // Si es líder, sigue los puntos de patrullaje
        if (isLeader)
        {
            UpdatePatrolPoint();
        }
        // Si no es líder, aplica las reglas de flocking
        else
        {
            if (Random.Range(0, 100) < 10)
            {
                ApplyFlockingRules();
            }
        }

        // Movimiento general del fantasma
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    void ApplyFlockingRules()
    {
        GameObject[] gos = FlockManager.FM.allGhost;
        List<GameObject> leaders = FlockManager.FM.leaders;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if (nDistance < FlockManager.FM.neighbourDistance)
                {
                    float influence = leaders.Contains(go) ? 2f : 1f; // Mayor influencia si es líder

                    vcentre += go.transform.position * influence;
                    groupSize += (int)influence;

                    if (nDistance < 1.0f)
                    {
                        vavoid += (this.transform.position - go.transform.position) * influence;
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed += anotherFlock.speed * influence;
                }
            }
        }

        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (FlockManager.FM.goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - this.transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      FlockManager.FM.rotationSpeed * Time.deltaTime);
            }
        }
    }

    void UpdatePatrolPoint()
    {
        // Verificar si el líder está cerca del punto de patrullaje actual
        if (Vector3.Distance(transform.position, FlockManager.FM.patrolPoints[currentPatrolPoint].transform.position) < 1.0f)
        {
            // Cambiar al siguiente punto de patrullaje
            currentPatrolPoint = (currentPatrolPoint + 1) % FlockManager.FM.patrolPoints.Length;
        }

        // Moverse hacia el próximo punto de patrullaje
        Vector3 directionToNextPoint = (FlockManager.FM.patrolPoints[currentPatrolPoint].transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToNextPoint);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, FlockManager.FM.rotationSpeed * Time.deltaTime);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
