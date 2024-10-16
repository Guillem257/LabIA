using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script should be attached to the ManagerFlocking object in your scene.
// Ensure that you assign the Ghost prefab in the Inspector under the variable 'ghostPrefab'.

public class FlockManager : MonoBehaviour
{
    // Static instance of FlockManager, allowing access from any Flock script.
    public static FlockManager FM; 
    
    // The ghost prefab to instantiate for each ghost in the flock
    public GameObject ghostPrefab1; 
    public GameObject ghostPrefab2; 
    public GameObject ghostPrefab3; 
    public GameObject ghostPrefab4; 
    public GameObject ghostPrefab5; 
    public GameObject ghostPrefab6;

    private GameObject ghost;

    // Number of ghosts in the flock
    public int numGhosts = 100; 
    public int numGhostLeads = 2;

    public int leadGhostInfluence = 4;
    
    // Array to store all instantiated ghosts
    public GameObject[] allGhosts; 
    
    // Defines the 3D space within which the ghosts can swim
    public Vector3 swimLimits = new Vector3(5, 5, 5); 
    
    // Randomly selected goal position for ghosts to move towards
    public Vector3 goalPos = Vector3.zero;

    [Header("Ghost Settings")] // Organizes the following variables in the Inspector for easier configuration
    
    // Minimum speed of the ghost (adjustable in Inspector)
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    
    // Maximum speed of the ghost (adjustable in Inspector)
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    
    // Distance at which ghosts recognize each other as neighbors (for cohesion, separation, and alignment)
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    
    // Rotation speed of the ghost when changing direction (adjustable in Inspector)
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;

    // Start is called before the first frame update
    // This method initializes the ghosts by spawning them within the defined limits
    void Start()
    {
        ghost = ghostPrefab1;
        // Initialize the array to hold all the ghosts
        allGhosts = new GameObject[numGhosts];
        int leadIndex = 0; 

        // Loop to create and place each ghost randomly within the swim limits
        for (int i = 0; i < numGhosts; i++)
        {
            // Calculate a random position within the swim limits
            Vector3 pos = this.transform.position + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x),
                0, //Random.Range(-swimLimits.y, swimLimits.y),  
                Random.Range(-swimLimits.z, swimLimits.z));

            int type = Random.Range(0, 6);
            switch (type)
            {
                case 0:
                    ghost = ghostPrefab1;
                    break;
                case 1:
                    ghost = ghostPrefab2;
                    break;
                case 2:
                    ghost = ghostPrefab3;
                    break;
                case 3:
                    ghost = ghostPrefab4;
                    break;
                case 4:
                    ghost = ghostPrefab5;
                    break;
                case 5:
                    ghost = ghostPrefab6;
                    break;
            }
            // Instantiate the ghost prefab at the random position with no rotation
            allGhosts[i] = Instantiate(ghost, pos, Quaternion.identity);

            if(leadIndex <= numGhostLeads - 1)
            {
                allGhosts[i].GetComponent<Flock>().isGhostLead = true;
                leadIndex++;
            }
            if(!allGhosts[i].GetComponent<Flock>().isGhostLead)
            {
                allGhosts[i].GetComponent<ParticleSystem>().Stop();
            }

        }

        // Set the static reference to this instance of FlockManager
        FM = this;
        
        // Initialize the goal position as the FlockManager's position
        if(numGhostLeads > 0)
            goalPos = allGhosts[0].transform.position;
    }   
}