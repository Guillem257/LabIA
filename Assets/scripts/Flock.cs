using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    float speed;
    bool turning = false;
    public bool isGhostLead = false;

    void Start()
    {
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
        if(isGhostLead)
        {
            speed = FlockManager.FM.maxSpeed;
        }
    }

    void Update()
    {
        Bounds b = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.swimLimits * 2);

        if (!b.Contains(transform.position))
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if (turning)
        {
            Vector3 direction = FlockManager.FM.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockManager.FM.rotationSpeed * Time.deltaTime);
        }
        else
        {
            // if (Random.Range(0, 100) < 10)
            // {
            //     speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
            // }

            // if (Random.Range(0, 100) < 10)
            // {
            //     ApplyFlockingRules();
            // }

            if( isGhostLead )
            {
                MoveGhostLead();
            }
            else
            {
                ApplyFlockingRules();
            }
        }

        // Mover el pez hacia adelante en el eje X y Z, manteniendo la posición Y constante
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    void ApplyFlockingRules()
    {
        GameObject[] gos = FlockManager.FM.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        Flock nearestGhostLead = null;
        float minGhostLeadDistance = Mathf.Infinity;
        
        foreach (GameObject go in gos)
        {
            Flock anotherFlock = go.GetComponent<Flock>();

            if(anotherFlock.isGhostLead)
            {
                float distance = Vector3.Distance(go.transform.position, this.transform.position);
                if(distance < minGhostLeadDistance)
                {
                    minGhostLeadDistance = distance;
                    nearestGhostLead = anotherFlock;
                }
            }

            if (go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if (nDistance < FlockManager.FM.neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if (nDistance < 1.0f)
                    {
                        vavoid += this.transform.position - go.transform.position;
                    }

                    gSpeed += anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (FlockManager.FM.goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            if (speed > FlockManager.FM.maxSpeed)
            {
                speed = FlockManager.FM.maxSpeed;
            }

            if(nearestGhostLead != null && minGhostLeadDistance < FlockManager.FM.leadGhostInfluence)
            {
                vcentre = nearestGhostLead.transform.position;
            }

            Vector3 direction = (vcentre + vavoid) - this.transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockManager.FM.rotationSpeed * Time.deltaTime);
            }
        }
    }
    public void MoveGhostLead(){
        if(Random.Range(0, 100) < 10)
        {
            FlockManager.FM.goalPos = FlockManager.FM.transform.position + new Vector3(
                Random.Range(-FlockManager.FM.swimLimits.x, FlockManager.FM.swimLimits.x),
                Random.Range(-FlockManager.FM.swimLimits.y, FlockManager.FM.swimLimits.y),
                Random.Range(-FlockManager.FM.swimLimits.z, FlockManager.FM.swimLimits.z));
        }

        Vector3 direction = FlockManager.FM.goalPos - transform.position;

        if(direction != Vector3.zero)        
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockManager.FM.rotationSpeed * Time.deltaTime);
    }
        
}