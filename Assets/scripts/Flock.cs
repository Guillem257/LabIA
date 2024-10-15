using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    float speed;
    bool turning = false;

    void Start()
    {
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
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
            direction.y = 0; // Ignorar el eje Y en la dirección
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockManager.FM.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
            {
                speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
            }

            if (Random.Range(0, 100) < 10)
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

        foreach (GameObject go in gos)
        {
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

                    Flock anotherFlock = go.GetComponent<Flock>();
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

            Vector3 direction = (vcentre + vavoid) - this.transform.position;
            direction.y = 0; // Ignorar el eje Y en la dirección
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockManager.FM.rotationSpeed * Time.deltaTime);
            }
        }
    }
}