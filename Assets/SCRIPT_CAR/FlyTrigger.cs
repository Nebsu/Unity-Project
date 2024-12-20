using UnityEngine;

public class FlyTrigger : MonoBehaviour
{
    public float flyDuration = 30f; // Durée pendant laquelle "fly" est true

    private void OnTriggerEnter(Collider other)
    {
        // Cherche le composant ShockWave sur l'objet ou son parent
        ShockWave shockWave = other.GetComponentInParent<ShockWave>();

        if (shockWave != null)
        {
            // Lance la coroutine pour activer fly dans ShockWave
            shockWave.StartCoroutine(shockWave.ActivateFlyForDuration(flyDuration));

            // Détruit l'objet FlyTrigger
            Destroy(gameObject);
            Debug.Log("FlyTrigger détruit après activation !");
        }
    }
}
