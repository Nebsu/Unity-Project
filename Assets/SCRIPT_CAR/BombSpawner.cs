using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [Header("Bomb Spawner Settings")]
    public GameObject bombPrefab;          // Prefab de la bombe ("Mon_00")
    public float respawnTime = 12f;        // Temps de respawn en secondes (12 secondes)

    private List<Transform> spawnPoints = new List<Transform>(); // Liste des points de spawn
    private Dictionary<Transform, GameObject> activeBombs = new Dictionary<Transform, GameObject>(); // Suivi des bombes actives

    private void Start()
    {
        // Récupérer tous les objets "Bomb_Spawner" dans la scène
        GameObject[] spawnerObjects = GameObject.FindGameObjectsWithTag("Bomb_Spawner");

        foreach (GameObject spawner in spawnerObjects)
        {
            spawnPoints.Add(spawner.transform);
        }

        // Vérifie que des points de spawn ont été trouvés
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("Aucun point de spawn trouvé dans la scène avec le tag 'Bomb_Spawner' !");
            return;
        }

        // Générer les bombes initiales sur chaque point de spawn
        SpawnInitialBombs();
    }

    // Génération initiale des bombes sur chaque spawner
    private void SpawnInitialBombs()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            SpawnBomb(spawnPoint);
        }
    }

    // Spawn une bombe sur un point donné
    private void SpawnBomb(Transform spawnPoint)
    {
        if (spawnPoint == null) return;

        if (bombPrefab == null)
        {
            Debug.LogError("Le prefab de bombe ('bombPrefab') n'est pas assigné !");
            return;
        }

        // Instancie la bombe et la stocke dans le dictionnaire
        GameObject newBomb = Instantiate(bombPrefab, spawnPoint.position, spawnPoint.rotation);
        activeBombs[spawnPoint] = newBomb;

        // Ajoute un script de gestion de destruction à la bombe
        Bomb bombScript = newBomb.AddComponent<Bomb>();
        bombScript.onDestroyed += () => StartCoroutine(RespawnBomb(spawnPoint));
    }

    // Coroutine pour régénérer une bombe sur un point donné après un délai
    private IEnumerator RespawnBomb(Transform spawnPoint)
    {
        yield return new WaitForSeconds(respawnTime);

        // Vérifie que la bombe n'existe plus avant de la respawner
        if (!activeBombs.ContainsKey(spawnPoint) || activeBombs[spawnPoint] == null)
        {
            SpawnBomb(spawnPoint);
        }
    }
}

// Classe pour gérer les événements de destruction d'une bombe
public class Bomb : MonoBehaviour
{
    public delegate void BombDestroyed();
    public event BombDestroyed onDestroyed;

    private void OnDestroy()
    {
        // Notifie que l'objet a été détruit
        if (onDestroyed != null)
        {
            onDestroyed.Invoke();
        }
    }
}
