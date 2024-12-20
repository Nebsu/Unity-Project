using UnityEngine;
using UnityEngine.UI; // Pour UI.Text
using TMPro;          // Si vous utilisez TextMeshPro

public class UIManager : MonoBehaviour
{
    [Header("Références")]
    public PrometeoCarController carController; // Référence au script PrometeoCarController
    public Slider speedGaugeSlider;             // Référence à la jauge de vitesse
    public TMP_Text speedValueText;             // Affiche la vitesse actuelle
    public TMP_Text snackCountText;             // Affiche le nombre de snacks restants
    public TMP_Text bombCountText;

    [Header("Options de jauge")]
    public float maxSpeed = 100f; // Valeur maximale de la jauge de vitesse

    [Header("Boost Gauge Configuration")]
    public Slider boostGaugeSlider;             // Référence à la jauge de boost
    public float maxBoost = 100f;               // Valeur maximale pour le boost

    [Header("Référence au SnackBooster")]
    public SnackBooster snackBooster; // Référence au script SnackBooster pour obtenir le nombre de snacks
    public DropBomb bombSpawner;

    public ShockWave shockwave;

    [Header("Fly Configuration")]
    public GameObject flyON;           // Référence au GameObject "FlyON" dans le Canvas

    void Start()
    {
        // Configuration initiale du Slider de vitesse
        if (speedGaugeSlider != null)
        {
            speedGaugeSlider.minValue = 0f;
            speedGaugeSlider.maxValue = maxSpeed;
        }

        // Configuration initiale du Slider de boost
        if (boostGaugeSlider != null)
        {
            boostGaugeSlider.minValue = 0f;
            boostGaugeSlider.maxValue = maxBoost;
            boostGaugeSlider.gameObject.SetActive(false); // Masquer la jauge par défaut
        }

        // Vérification des références
        if (carController == null)
            Debug.LogError("Le script 'PrometeoCarController' n'est pas assigné dans UIManager.");

        if (speedValueText == null)
            Debug.LogError("Le texte de la vitesse n'est pas assigné dans UIManager.");

        if (snackCountText == null)
            Debug.LogError("Le texte pour le nombre de snacks n'est pas assigné dans UIManager.");

        if (snackBooster == null)
            Debug.LogError("Le script 'SnackBooster' n'est pas assigné dans UIManager.");

        if (bombSpawner == null)
            Debug.LogError("Le script 'BombSpawner' n'est pas assigné dans UIManager.");

        if (shockwave == null)
            Debug.LogError("Le script 'ShockWave' n'est pas assigné dans UIManager.");

        if (flyON == null)
            Debug.LogError("Le GameObject 'FlyON' n'est pas assigné dans UIManager.");

        if (boostGaugeSlider == null)
            Debug.LogError("La jauge de Boost n'est pas assignée dans UIManager.");
    }

    void Update()
    {
        // Mise à jour de la jauge de vitesse
        if (carController != null && speedGaugeSlider != null)
        {
            float currentSpeed = Mathf.Abs(carController.carSpeed);
            speedGaugeSlider.value = currentSpeed;

            // Mise à jour du texte avec la valeur de la vitesse
            if (speedValueText != null)
            {
                speedValueText.text = Mathf.RoundToInt(currentSpeed).ToString() + " km/h";
            }
        }

        // Mise à jour du texte pour le nombre de snacks
        if (snackBooster != null && snackCountText != null)
        {
            snackCountText.text = ": " + snackBooster.NB.ToString();
        }

        if (bombSpawner != null && bombCountText != null)
        {
            bombCountText.text = ": " + bombSpawner.NB.ToString();
        }

        // Gestion de l'état du GameObject "FlyON" et de la jauge Boost
        if (shockwave != null)
        {
            if (flyON != null)
            {
                flyON.SetActive(shockwave.fly); // Active ou désactive "FlyON"
            }

            if (boostGaugeSlider != null)
            {
                boostGaugeSlider.gameObject.SetActive(shockwave.fly); // Affiche la jauge seulement si fly est true

                // Mise à jour de la valeur de la jauge de Boost uniquement si elle est active
                if (shockwave.fly)
                {
                    boostGaugeSlider.value = shockwave.boost;
                }
            }
        }
    }
}
