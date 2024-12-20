using UnityEngine;

public class SnackPickup : MonoBehaviour
{
    [Header("Effet du Snack")]
    public int snackValue = 1; // Nombre de snacks ajoutés lorsqu'on ramasse cet objet

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet entrant possède le script SnackBooster
        SnackBooster snackBooster = other.GetComponentInParent<SnackBooster>();

        if (snackBooster != null)
        {
            // Augmente le nombre de snacks
            snackBooster.NB += snackValue;
            Debug.Log("Snack ramassé ! Snacks disponibles : " + snackBooster.NB);

            // Détruit l'objet ramassé
            Destroy(gameObject);
        }
    }
}