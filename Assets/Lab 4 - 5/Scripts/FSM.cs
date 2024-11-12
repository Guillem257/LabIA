using UnityEngine;
using System.Collections;

public class FSM : MonoBehaviour
{
    public Transform cop;
    public GameObject treasure;
    public float dist2Steal = 10f;
    Moves moves;
    UnityEngine.AI.NavMeshAgent agent;

    private WaitForSeconds wait = new WaitForSeconds(0.05f);
    delegate IEnumerator State();
    private State state;

    private bool hasReachedHidingSpot = false; 
    private bool isHiding = false;            

    IEnumerator Start()
    {
        moves = gameObject.GetComponent<Moves>();
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        yield return wait;

        state = Wander;

        while (enabled)
            yield return StartCoroutine(state());
    }

    IEnumerator Wander()
    {
        Debug.Log("Wander state");

        while (Vector3.Distance(cop.position, treasure.transform.position) < dist2Steal)
        {
            moves.Wander();
            yield return wait;
        }

        state = Approaching;
    }

    IEnumerator Approaching()
    {
        Debug.Log("Approaching state");

        agent.speed = 2f;
        moves.Seek(treasure.transform.position);

        bool stolen = false;
        while (Vector3.Distance(cop.position, treasure.transform.position) > dist2Steal)
        {
            if (Vector3.Distance(treasure.transform.position, transform.position) < 2f)
            {
                stolen = true;
                break;
            }
            yield return wait;
        }

        if (stolen)
        {
            treasure.GetComponent<Renderer>().enabled = false;
            Debug.Log("Stolen");
            state = Hiding;
        }
        else
        {
            agent.speed = 1f;
            state = Wander;
        }
    }

    IEnumerator Hiding()
    {
        Debug.Log("Hiding state");

        if (!hasReachedHidingSpot || Vector3.Distance(transform.position, cop.position) < moves.safeDistanceFromCop)
        {
            moves.Hide();
            hasReachedHidingSpot = false; // Reiniciamos el estado para buscar un nuevo escondite
            isHiding = false;
        }

        while (true)
        {
            if (!hasReachedHidingSpot && Vector3.Distance(transform.position, agent.destination) < 1.0f)
            {
                hasReachedHidingSpot = true;
                isHiding = true;
                Debug.Log("Reached hiding spot, now hiding.");
            }

            if (isHiding && Vector3.Distance(transform.position, cop.position) < moves.safeDistanceFromCop)
            {
                Debug.Log("Cop too close! Finding new hiding spot.");
                hasReachedHidingSpot = false;
                isHiding = false;
                moves.Hide();
            }

            yield return wait;
        }
    }
}
