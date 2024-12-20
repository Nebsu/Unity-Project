using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CarSpawner : MonoBehaviour
{
    [Header("Car Spawner Settings")]
    public GameObject carPrefab;           // Le prefab de la voiture à instancier
    public float spawnInterval = 20f;      // Intervalle de spawn en secondes
    private Transform[] spawnPoints;       // Tableau des points d'apparition
    private GameObject player;             // Référence au joueur
    private bool isSpawning = false;       // Vérifie si une voiture est déjà en cours de génération

    private void Start()
    {
        // Initialisation des points de spawn
        InitializeSpawnPoints();

        // Récupère la référence au joueur
        player = GameObject.Find("Prometheus");
        if (player == null)
        {
            Debug.LogError("Joueur 'Prometheus' non trouvé dans la scène !");
        }

        // Démarre la coroutine pour générer une voiture à intervalles réguliers
        StartCoroutine(SpawnCarRoutine());
    }

    // Initialisation des points de spawn
    private void InitializeSpawnPoints()
    {
        spawnPoints = new Transform[4];
        spawnPoints[0] = GameObject.Find("Car_Spawner").transform;
        spawnPoints[1] = GameObject.Find("Car_Spawner (1)").transform;
        spawnPoints[2] = GameObject.Find("Car_Spawner (2)").transform;
        spawnPoints[3] = GameObject.Find("Car_Spawner (3)").transform;

        // Vérifie que tous les points de spawn ont bien été trouvés
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint == null)
            {
                Debug.LogError("Un ou plusieurs points de spawn sont introuvables dans la scène !");
            }
        }
    }

    // Coroutine pour gérer l'apparition des voitures
    IEnumerator SpawnCarRoutine()
    {
        while (true) // Boucle infinie pour faire spawn les voitures
        {
            if (!isSpawning) // Vérifie qu'aucune voiture n'est en cours de génération
            {
                isSpawning = true; // Bloque les autres spawns pendant cette exécution
                SpawnCarAtFarthestPoint(); // Génère une voiture
                isSpawning = false; // Libère le blocage
            }
            yield return new WaitForSeconds(spawnInterval); // Attente avant le prochain cycle
        }
    }

    // Fonction pour instancier une voiture au point de spawn le plus éloigné
    private void SpawnCarAtFarthestPoint()
    {
        if (carPrefab == null || spawnPoints.Length == 0 || player == null)
        {
            Debug.LogError("Problème avec CarPrefab, spawnPoints ou joueur.");
            return;
        }

        // Trouve le point de spawn le plus éloigné
        Transform farthestSpawnPoint = GetFarthestSpawnPoint();
        if (farthestSpawnPoint == null)
        {
            Debug.LogError("Aucun point de spawn valide n'a été trouvé !");
            return;
        }

        // Instancie une voiture au point le plus éloigné
        GameObject newCar = Instantiate(carPrefab, farthestSpawnPoint.position, farthestSpawnPoint.rotation);

        // Vérifie que le NavMeshAgent fonctionne correctement
        NavMeshAgent agent = newCar.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(farthestSpawnPoint.position); // Positionne correctement la voiture sur le NavMesh
        }

        Debug.Log("Voiture générée au point : " + farthestSpawnPoint.name);
    }

    // Fonction pour trouver le point de spawn le plus éloigné du joueur
    private Transform GetFarthestSpawnPoint()
    {
        Transform farthestPoint = null;
        float maxDistance = float.MinValue;

        foreach (Transform spawnPoint in spawnPoints)
        {
            float distance = Vector3.Distance(player.transform.position, spawnPoint.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestPoint = spawnPoint;
            }
        }

        return farthestPoint;
    }
}
