using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableSpawner : MonoBehaviour
{
    [Header("Consumable Spawner Settings")]
    public GameObject chickenLegPrefab;   // Prefab de l'objet "chicken leg piece"
    public GameObject comboFullRedPrefab; // Prefab de l'objet "Combo_Full_Red"
    public float respawnTime = 12f;      // Temps de respawn en secondes (2 minutes)

    private List<Transform> spawnPoints = new List<Transform>(); // Liste des points de spawn
    private Dictionary<Transform, GameObject> activeConsumables = new Dictionary<Transform, GameObject>(); // Suivi des objets actifs

    private void Start()
    {
        // Récupérer tous les objets "Consumable_Spawner" dans la scène
        GameObject[] spawnerObjects = GameObject.FindGameObjectsWithTag("Consumable_Spawner");

        foreach (GameObject spawner in spawnerObjects)
        {
            spawnPoints.Add(spawner.transform);
        }

        // Vérifie que des points de spawn ont été trouvés
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("Aucun point de spawn trouvé dans la scène avec le tag 'Consumable_Spawner' !");
            return;
        }

        // Générer les objets initiaux sur chaque point de spawn
        SpawnInitialConsumables();
    }

    // Génération initiale des objets sur chaque spawner
    private void SpawnInitialConsumables()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            SpawnConsumable(spawnPoint);
        }
    }

    // Spawn un consommable sur un point donné
    private void SpawnConsumable(Transform spawnPoint)
    {
        if (spawnPoint == null) return;

        // Choisir aléatoirement un des deux prefabs à instancier
        GameObject prefabToSpawn = Random.value > 0.5f ? chickenLegPrefab : comboFullRedPrefab;

        if (prefabToSpawn == null)
        {
            Debug.LogError("Prefab non assigné pour 'chickenLegPrefab' ou 'comboFullRedPrefab' !");
            return;
        }

        // Instancie l'objet et le stocke dans le dictionnaire
        GameObject newConsumable = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        activeConsumables[spawnPoint] = newConsumable;

        // Abonne l'objet pour écouter sa destruction
        Consumable consumableScript = newConsumable.AddComponent<Consumable>();
        consumableScript.onDestroyed += () => StartCoroutine(RespawnConsumable(spawnPoint));
    }

    // Coroutine pour régénérer un consommable sur un point donné après un délai
    private IEnumerator RespawnConsumable(Transform spawnPoint)
    {
        yield return new WaitForSeconds(respawnTime);

        // Assurez-vous que l'objet n'existe plus avant de respawner
        if (!activeConsumables.ContainsKey(spawnPoint) || activeConsumables[spawnPoint] == null)
        {
            SpawnConsumable(spawnPoint);
        }
    }
}

// Classe pour gérer les événements de destruction d'un consommable
public class Consumable : MonoBehaviour
{
    public delegate void ConsumableDestroyed();
    public event ConsumableDestroyed onDestroyed;

    private void OnDestroy()
    {
        // Notifie que l'objet a été détruit
        if (onDestroyed != null)
        {
            onDestroyed.Invoke();
        }
    }
}
