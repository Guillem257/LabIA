using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script should be attached to the ManagerFlocking object in your scene.
// Ensure that you assign the Fish prefab in the Inspector under the variable 'fishPrefab'.

public class FlockManager : MonoBehaviour
{
    // Static instance of FlockManager, allowing access from any Flock script.
    public static FlockManager FM; 
    
    // The fish prefab to instantiate for each fish in the flock
    public GameObject fishPrefab1; 
    public GameObject fishPrefab2; 
    public GameObject fishPrefab3; 
    public GameObject fishPrefab4; 
    public GameObject fishPrefab5; 
    public GameObject fishPrefab6;

    private GameObject ghost;

    // Number of fish in the flock
    public int numFish = 6; 
    
    // Array to store all instantiated fish
    public GameObject[] allFish; 
    
    // Defines the 3D space within which the fish can swim
    public Vector3 swimLimits = new Vector3(5, 5, 5); 
    
    // Randomly selected goal position for fish to move towards
    public Vector3 goalPos = Vector3.zero;

    [Header("Fish Settings")] // Organizes the following variables in the Inspector for easier configuration
    
    // Minimum speed of the fish (adjustable in Inspector)
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    
    // Maximum speed of the fish (adjustable in Inspector)
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    
    // Distance at which fish recognize each other as neighbors (for cohesion, separation, and alignment)
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    
    // Rotation speed of the fish when changing direction (adjustable in Inspector)
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;

    // Start is called before the first frame update
    // This method initializes the fish by spawning them within the defined limits
    void Start()
    {
        ghost = fishPrefab1;
        // Initialize the array to hold all the fish
        allFish = new GameObject[numFish];

        // Loop to create and place each fish randomly within the swim limits
        for (int i = 0; i < numFish; i++)
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
                    ghost = fishPrefab1;
                    break;
                case 1:
                    ghost = fishPrefab2;
                    break;
                case 2:
                    ghost = fishPrefab3;
                    break;
                case 3:
                    ghost = fishPrefab4;
                    break;
                case 4:
                    ghost = fishPrefab5;
                    break;
                case 5:
                    ghost = fishPrefab6;
                    break;
            }
            // Instantiate the fish prefab at the random position with no rotation
            allFish[i] = Instantiate(ghost, pos, Quaternion.identity);


        }

        // Set the static reference to this instance of FlockManager
        FM = this;
        
        // Initialize the goal position as the FlockManager's position
        goalPos = this.transform.position;
    }

    // Update is called once per frame
    // Randomly updates the goal position for fish to swim towards
    void Update()
    {
        // Occasionally change the goal position (10% chance each frame)
        if (Random.Range(0, 100) < 10)
        {
            // Calculate a new random goal position within the swim limits
            goalPos = this.transform.position + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x),
                //Random.Range(-swimLimits.y, swimLimits.y),  
                Random.Range(-swimLimits.z, swimLimits.z));
        }
    }
}
