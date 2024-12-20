using UnityEngine;
using TMPro; // Import du namespace TextMeshPro

public class Chrono : MonoBehaviour
{
    public TMP_Text chronoText; // Utilisation de TMP_Text pour TextMeshPro
    private float timeElapsed = 0f;
    private bool isPaused = false; // Indicateur de pause

    void Update()
    {
        // Incrémenter le temps écoulé uniquement si non en pause
        if (!isPaused)
        {
            timeElapsed += Time.deltaTime;

            // Convertir en minutes et secondes
            int minutes = Mathf.FloorToInt(timeElapsed / 60);
            int seconds = Mathf.FloorToInt(timeElapsed % 60);

            // Afficher dans le TextMeshPro UI
            if (chronoText != null)
            {
                chronoText.text = $"{minutes:D2}:{seconds:D2}";
            }
        }
    }

    // Méthode pour mettre en pause ou reprendre le chrono
    public void PauseChrono(bool pause)
    {
        isPaused = pause;
    }

    // Optionnel : Méthode pour récupérer le temps écoulé
    public float GetElapsedTime()
    {
        return timeElapsed;
    }
}
