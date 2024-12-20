using UnityEngine;

public class SpeedBoostTrigger : MonoBehaviour
{
    [Header("Multiplicateur de vitesse")]
    public float speedMultiplier = 2f; // Multiplicateur pour la vitesse actuelle

    [Header("Durée du boost (en secondes)")]
    public float boostDuration = 5f; // Durée du boost en secondes

    private void OnTriggerEnter(Collider other)
    {   
        print("Trigger activé!");

        // Vérifie si l'objet entrant possède un Rigidbody
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();
        print(other.gameObject.name);

        if (rb != null)
        {
            print("toto");
            // Applique le boost temporaire
            StartCoroutine(ApplySpeedBoost(rb));
            print("Boost appliqué à : " + other.gameObject.name);

            // Détruit l'objet possédant ce script
            Destroy(gameObject);
            print("Objet détruit après le trigger.");
        }
    }

    private System.Collections.IEnumerator ApplySpeedBoost(Rigidbody carRigidbody)
    {
        // Sauvegarde de la vélocité actuelle
        Vector3 originalVelocity = carRigidbody.velocity;

        // Applique le boost de vitesse actuelle
        carRigidbody.velocity *= speedMultiplier;

        print("Vitesse actuelle boostée : " + carRigidbody.velocity.magnitude);

        // Attend la durée du boost
        yield return new WaitForSeconds(boostDuration);

        // Restaure la vélocité d'origine
        carRigidbody.velocity = originalVelocity;

        print("Boost terminé, vitesse restaurée.");
    }
}
