using System.Collections;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;
namespace BBUnity.Actions{

    [Action("MyActions/HideAction")]
    [Help("Hides from the cop if too close.")]
    public class HideAction : GOAction
    {
        [InParam("agent")]
        public GameObject agent;

        [InParam("cop")]
        public Transform cop;

        [InParam("safeDistanceFromCop")]
        public float safeDistanceFromCop = 10f;

        [InParam("waitTime")]
        public float waitTime = 0.1f;

        [OutParam("hidingSpot")]
        public Vector3 hidingSpot;

        private bool hasReachedHidingSpot = false;
        private bool isHiding = false;
        private float nextCheckTime = 0f;

        public override void OnStart()
        {
            base.OnStart();
            //Hiding();
        }

        public override TaskStatus OnUpdate()
        {
            GameObject[] hidingSpots = GameObject.FindGameObjectsWithTag("Hide");
            if (Time.time >= nextCheckTime)
            {
                nextCheckTime = Time.time + waitTime;
                //Hiding();
                float dist = Mathf.Infinity;
            Vector3 chosenSpot = Vector3.zero;
            Vector3 chosenDir = Vector3.zero;
            GameObject chosenGO = null;

            for (int i = 0; i < hidingSpots.Length; i++)
            {
                float distanceToCop = Vector3.Distance(hidingSpots[i].transform.position, cop.transform.position);
                if (distanceToCop >= safeDistanceFromCop)
                {
                    Vector3 hideDir = hidingSpots[i].transform.position - cop.transform.position;
                    Vector3 hidePos = hidingSpots[i].transform.position + hideDir.normalized;

                    float distanceToThief = Vector3.Distance(gameObject.transform.position, hidePos);
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
                    //Seek(info.point + chosenDir.normalized);
                    hidingSpot = info.point + chosenDir.normalized;
                    Debug.Log("Hiding cop set to: " + info.point);
                }
            }
            else
            {
                Debug.LogWarning("No suitable hiding spot found far enough from the cop!");
            }
            }
            return TaskStatus.COMPLETED;
        }

        // private void Hiding()
        // {
        //     Debug.Log("Hiding state");

        //     if (!hasReachedHidingSpot || Vector3.Distance(agent.transform.position, cop.position) < safeDistanceFromCop)
        //     {
        //         FindNewHidingSpot(out hidingSpot);
        //         hasReachedHidingSpot = false; // Reiniciamos el estado para buscar un nuevo escondite
        //         isHiding = false;
        //     }

        //     if (!hasReachedHidingSpot && Vector3.Distance(agent.transform.position, hidingSpot) < 1.0f)
        //     {
        //         hasReachedHidingSpot = true;
        //         isHiding = true;
        //         Debug.Log("Reached hiding spot, now hiding.");
        //     }

        //     if (isHiding && Vector3.Distance(agent.transform.position, cop.position) < safeDistanceFromCop)
        //     {
        //         Debug.Log("Cop too close! Finding new hiding spot.");
        //         hasReachedHidingSpot = false;
        //         isHiding = false;
        //         FindNewHidingSpot(out hidingSpot);
        //     }
        // }

        private void FindNewHidingSpot(out Vector3 hidingSpot)
        {
            // Implementa la lógica para encontrar un nuevo escondite
            hidingSpot = agent.transform.position + (agent.transform.position - cop.position).normalized * safeDistanceFromCop;
            Debug.Log("New hiding spot set.");
        }
    }
}