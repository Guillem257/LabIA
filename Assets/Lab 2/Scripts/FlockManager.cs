// using System.Collections.Generic;
// using UnityEngine;

// public class FlockManager2 : MonoBehaviour
// {
//     public static FlockManager2 FM;

//     // Prefabs de los fantasmas
//     public GameObject ghostPrefab1;
//     public GameObject ghostPrefab2;
//     public GameObject ghostPrefab3;
//     public GameObject ghostPrefab4;
//     public GameObject ghostPrefab5;
//     public GameObject ghostPrefab6;
//     public GameObject ghostPrefab7; // Prefab de los líderes

//     // Número de fantasmas
//     public int numghost = 6;

//     // Lista de todos los fantasmas
//     public GameObject[] allGhost;
//     public List<GameObject> leaders = new List<GameObject>(); // Lista de líderes

//     // Configuración del área de patrullaje
//     public Vector3 swimLimits = new Vector3(5, 5, 5);
//     public GameObject[] patrolPoints; // Puntos de patrullaje

//     // Posición del objetivo para los fantasmas
//     public Vector3 goalPos = Vector3.zero;

//     [Header("Configuraciones de los Fantasmas")]
//     [Range(0.0f, 5.0f)]
//     public float minSpeed;
//     [Range(0.0f, 5.0f)]
//     public float maxSpeed;
//     [Range(1.0f, 10.0f)]
//     public float neighbourDistance;
//     [Range(0.0f, 5.0f)]
//     public float rotationSpeed;

//     void Start()
//     {
//         // Inicializa la lista de fantasmas
//         allGhost = new GameObject[numghost];

//         for (int i = 0; i < numghost; i++)
//         {
//             Vector3 pos = this.transform.position + new Vector3(
//                 Random.Range(-swimLimits.x, swimLimits.x),
//                 0,
//                 Random.Range(-swimLimits.z, swimLimits.z));

//             GameObject ghost; // DECLARACIÓN de la variable ghost

//             // Definir si el fantasma es un líder
//             if (i < 2)
//             {
//                 ghost = ghostPrefab7; // Usar prefab específico para los líderes
//                 GameObject leader = Instantiate(ghost, pos, Quaternion.identity);
//                 leaders.Add(leader); // Añadir líder a la lista
//                 allGhost[i] = leader; // Añadir líder al array de fantasmas
//             }
//             else
//             {
//                 // Seleccionar prefab aleatoriamente para los seguidores
//                 int type = Random.Range(0, 6);
//                 switch (type)
//                 {
//                     case 0:
//                         ghost = ghostPrefab1;
//                         break;
//                     case 1:
//                         ghost = ghostPrefab2;
//                         break;
//                     case 2:
//                         ghost = ghostPrefab3;
//                         break;
//                     case 3:
//                         ghost = ghostPrefab4;
//                         break;
//                     case 4:
//                         ghost = ghostPrefab5;
//                         break;
//                     case 5:
//                         ghost = ghostPrefab6;
//                         break;
//                     default:
//                         ghost = ghostPrefab1;
//                         break;
//                 }
//                 allGhost[i] = Instantiate(ghost, pos, Quaternion.identity);
//             }
//         }

//         FM = this;
//         goalPos = this.transform.position;
//     }

//     void Update()
//     {
//         // Cambiar ocasionalmente la posición objetivo
//         if (Random.Range(0, 100) < 10)
//         {
//             goalPos = this.transform.position + new Vector3(
//                 Random.Range(-swimLimits.x, swimLimits.x),
//                 0,
//                 Random.Range(-swimLimits.z, swimLimits.z));
//         }
//     }
// }
