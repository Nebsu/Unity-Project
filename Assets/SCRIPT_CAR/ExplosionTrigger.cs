using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ExplosionTrigger : MonoBehaviour
{
    [Header("Multiplicateur de vitesse")]
    public float speedMultiplier = 2f; // Multiplicateur pour la vitesse actuelle

    [Header("Durée du boost (en secondes)")]
    public float boostDuration = 5f; // Durée du boost en secondes

    [Header("Explosion Parameters")]
    public float upwardForce = 10f;    // Force additionnelle verticale pour un saut
    public float rotationTorque = 500f; // Force pour faire pivoter la voiture ou l'ennemi

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger activé!");

        // Récupère le Rigidbody dans l'objet ou ses parents
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();
        NavMeshAgent agent = other.GetComponentInParent<NavMeshAgent>();
        if (rb != null)
        {
             print("Objet en collision : " + other.gameObject.name);
            if (agent != null)
            {
                 agent.enabled = false;
                 rb.AddForce(rb.transform.forward*upwardForce, ForceMode.Impulse); 
            
            }

            // Applique les effets d'explosion
            ApplyJumpAndRotation(rb);

            // Applique le boost temporaire de vitesse si nécessaire
            StartCoroutine(ApplySpeedBoost(rb));
        }

        // Détruit l'objet possédant ce script
       
        print("Objet détruit après le trigger.");
    }

    private void ApplyJumpAndRotation(Rigidbody rb)
    {
        // Applique une force verticale pour simuler un saut
        rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
        print("Force verticale appliquée : " + upwardForce);

        // Applique une rotation aléatoire autour des trois axes
        Vector3 randomTorque = new Vector3(
            Random.Range(-rotationTorque, rotationTorque),  // Axe X
            Random.Range(-rotationTorque, rotationTorque),  // Axe Y
            Random.Range(-rotationTorque, rotationTorque)   // Axe Z
        );

        rb.AddTorque(randomTorque, ForceMode.Impulse);
        print("Rotation aléatoire appliquée : " + randomTorque);
    }

    private IEnumerator ApplySpeedBoost(Rigidbody rb)
    {
        // Sauvegarde de la vélocité actuelle
        Vector3 originalVelocity = rb.velocity;

        // Applique le boost de vitesse en multipliant la vélocité
        rb.velocity *= speedMultiplier;

        //this.gameObject.SetActive(false);
        print("Vitesse boostée temporairement : " + rb.velocity.magnitude);



        yield return new WaitForEndOfFrame();
        print("Frame skipped : " + rb.velocity.magnitude);


        // Attend la durée définie avant de restaurer la vitesse
        yield return new WaitForSeconds(boostDuration);

        NavMeshAgent agent = rb.GetComponentInParent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = true;
            }
                

        // Restaure la vélocité d'origine
        rb.velocity = originalVelocity;

        print("Boost terminé, vitesse restaurée.");

         Destroy(gameObject);
    }
}