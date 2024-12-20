using UnityEngine;

public class SnackBooster : MonoBehaviour
{
    [Header("Paramètres de Boost")]
    public float speedMultiplier = 1.5f; // Multiplicateur de la vitesse
    public float maxSpeedMultiplier = 1.5f; // Multiplicateur de la vitesse maximale
    public float boostDuration = 5f; // Durée du boost en secondes

    [Header("Nombre de Snacks")]
    public int NB = 3; // Nombre de snacks disponibles

    private bool isBoosting = false; // Pour éviter plusieurs boosts simultanés
    private PrometeoCarController carController; // Référence au contrôleur de la voiture
    private int originalMaxSpeed; // Stocke la valeur originale de maxSpeed

    void Start()
    {
        // On suppose que le script est attaché à la voiture
        carController = GetComponent<PrometeoCarController>();

        if (carController == null)
        {
            Debug.LogError("Le script SnackBooster nécessite un PrometeoCarController attaché au GameObject.");
        }
    }

    void Update()
    {
        // Détecte si E est pressé et qu'il reste des snacks disponibles
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryUseSnack();
        }
    }

    private void TryUseSnack()
    {
        if (NB > 0 && !isBoosting) // Vérifie s'il y a des snacks et si on n'est pas déjà en boost
        {
            NB--; // Consomme un snack
            StartCoroutine(ApplySpeedBoost());
            Debug.Log("Snack utilisé ! Snacks restants : " + NB);
        }
        else if (NB <= 0)
        {
            Debug.Log("Aucun snack disponible !");
        }
    }

    private System.Collections.IEnumerator ApplySpeedBoost()
    {
        isBoosting = true;

        // Sauvegarde des valeurs originales
        originalMaxSpeed = carController.maxSpeed; // maxSpeed est un int
        Vector3 originalVelocity = carController.GetComponent<Rigidbody>().velocity;

        // Applique les boosts
        carController.maxSpeed = Mathf.RoundToInt(originalMaxSpeed * maxSpeedMultiplier);
        carController.GetComponent<Rigidbody>().velocity *= speedMultiplier;

        Debug.Log("Boost activé ! Nouvelle vitesse max : " + carController.maxSpeed);

        // Attend la durée du boost
        yield return new WaitForSeconds(boostDuration);

        // Restaure les valeurs originales
        carController.maxSpeed = originalMaxSpeed;

        Debug.Log("Boost terminé. Vitesse max restaurée à : " + originalMaxSpeed);

        isBoosting = false;
    }
}
