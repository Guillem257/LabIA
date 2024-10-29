using UnityEngine;
using UnityEngine.AI;

public class Moves : MonoBehaviour
{
    public GameObject target;
    public Collider floor;
    public float safeDistanceFromCop = 10f;
    GameObject[] hidingSpots;
    NavMeshAgent agent;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        hidingSpots = GameObject.FindGameObjectsWithTag("Hide");
    }

    public void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    public void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }

    Vector3 wanderTarget = Vector3.zero;

    public void Wander()
    {
        float wanderRadius = 2;
        float wanderDistance = 4;
        float wanderJitter = 3;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = transform.TransformPoint(targetLocal);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetWorld, out hit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.Log("Wander target set to: " + hit.position);
        }
        else
        {
            Debug.Log("Wander target is out of NavMesh bounds.");
        }
    }

    public void Hide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = null;

        for (int i = 0; i < hidingSpots.Length; i++)
        {
            float distanceToCop = Vector3.Distance(hidingSpots[i].transform.position, target.transform.position);
            if (distanceToCop >= safeDistanceFromCop)
            {
                Vector3 hideDir = hidingSpots[i].transform.position - target.transform.position;
                Vector3 hidePos = hidingSpots[i].transform.position + hideDir.normalized;

                float distanceToThief = Vector3.Distance(this.transform.position, hidePos);
                if (distanceToThief < dist)
                {
                    chosenSpot = hidePos;
                    chosenDir = hideDir;
                    chosenGO = hidingSpots[i];
                    dist = distanceToThief;
                }
            }
        }

        if (chosenGO != null)
        {
            Collider hideCol = chosenGO.GetComponent<Collider>();
            Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
            RaycastHit info;
            float rayDistance = 250.0f;
            if (hideCol.Raycast(backRay, out info, rayDistance))
            {
                Seek(info.point + chosenDir.normalized);
                Debug.Log("Hiding target set to: " + info.point);
            }
        }
        else
        {
            Debug.LogWarning("No suitable hiding spot found far enough from the cop!");
        }
    }
}
