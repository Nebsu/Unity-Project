using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnnemyCarController : MonoBehaviour
{
    public NavMeshAgent agent;       // Le NavMeshAgent de la voiture
    public GameObject player;        // Le joueur

    [Header("Car Settings")]
    public float chaseSpeed = 10f;           // Vitesse de poursuite
    public float acceleration = 8f;          // Accélération de la voiture
    public float angularSpeed = 120f;        // Vitesse de rotation
    public float stoppingDistance = 2f;      // Distance d'arrêt par rapport au joueur

    [Header("Debug")]
    public Vector3 velocity;                 // Vélocité actuelle de la voiture

    private Vector3 previousPosition;        // Position précédente pour calculer la vélocité

    void Start()
    {
        // Récupère le joueur via son nom dans la scène
        player = GameObject.Find("Prometheus");

        // Configure les paramètres du NavMeshAgent
        if (agent != null)
        {
            agent.speed = chaseSpeed;             // Vitesse de poursuite
            agent.acceleration = acceleration;    // Accélération
            agent.angularSpeed = angularSpeed;    // Vitesse de rotation
            agent.stoppingDistance = stoppingDistance;  // Distance d'arrêt
        }
        else
        {
            Debug.LogError("NavMeshAgent manquant sur " + gameObject.name);
        }

        previousPosition = transform.position; // Initialise la position précédente
    }

    void Update()
    {
        // Si le joueur est défini, définir la destination
        if (player != null)
        {
            agent.SetDestination(player.transform.position);

            // Met à jour la vélocité en fonction du déplacement entre deux frames
            UpdateVelocity();
        }
        else
        {
            Debug.LogWarning("Joueur non trouvé dans la scène !");
        }
    }

    private void UpdateVelocity()
    {
        // Calcul de la vélocité : déplacement entre la position précédente et actuelle divisé par le temps
        velocity = (transform.position - previousPosition) / Time.deltaTime;

        // Met à jour la position précédente pour la prochaine frame
        previousPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Vérifie si la collision est avec le joueur
        if (collision.gameObject == player)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }
}