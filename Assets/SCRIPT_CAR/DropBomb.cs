using UnityEngine;

public class DropBomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    public GameObject bombPrefab;          // Référence au prefab de la bombe
    public float collisionReactivateTime = 1f;   // Temps avant de réactiver la collision
    public int NB = 3;                           // Nombre de bombes disponibles

    [Header("Spawn Offset")]
    public Vector3 spawnOffset = new Vector3(0, -1000f, 0); // Décalage pour éviter que la bombe spawn dans le sol

    void Start()
    {
        // Vérifie que le prefab est assigné dans l'inspecteur
        if (bombPrefab == null)
        {
            Debug.LogError("Le prefab 'bombPrefab' n'est pas assigné dans l'inspecteur !");
        }
    }

    void Update()
    {
        // Vérifie si la touche F est pressée et qu'il reste des bombes (NB > 0)
        if (Input.GetKeyDown(KeyCode.F))
        {
            TrySpawnBomb();
        }
    }

    private void TrySpawnBomb()
    {
        if (NB > 0 && bombPrefab != null)
        {
            // Décrémente le compteur de bombes
            NB--;

            // Calcule la position de spawn (position actuelle + offset)
            Vector3 spawnPosition = transform.position + spawnOffset;

            // Instantie une copie du prefab de la bombe
            GameObject spawnedBomb = Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Bombe spawnée ! Bombes restantes : " + NB);

            // Désactive temporairement la collision de la bombe
            Collider bombCollider = spawnedBomb.GetComponent<Collider>();
            if (bombCollider != null)
            {
                bombCollider.enabled = false; // Désactive la collision temporairement
                StartCoroutine(ReactivateCollision(bombCollider));
            }
        }
        else
        {
            Debug.Log("Aucune bombe restante ou prefab 'bombPrefab' non assigné !");
        }
    }

    private System.Collections.IEnumerator ReactivateCollision(Collider bombCollider)
    {
        // Attend le délai avant de réactiver la collision
        yield return new WaitForSeconds(collisionReactivateTime);

        // Réactive la collision
        bombCollider.enabled = true;
        Debug.Log("Collision de la bombe réactivée.");
    }
}
