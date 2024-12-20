using UnityEngine;

public class CarSoundController : MonoBehaviour
{
    public AudioSource engineSound;  // Référence à l'AudioSource pour le son du moteur
    public AudioSource driftingSound;  // Référence à l'AudioSource pour le son de dérive
    public PrometeoCarController carController;  // Référence au script qui contient la variable isDrifting

    private bool isAccelerating = false;  // Indicateur d'accélération
    private bool wasDrifting = false;    // Indicateur pour vérifier si la voiture a commencé à dériver

    void Start()
    {
        if (engineSound == null)
        {
            Debug.LogError("L'AudioSource pour le moteur n'est pas assignée !");
        }

        if (driftingSound == null)
        {
            Debug.LogError("L'AudioSource pour le son de dérive n'est pas assignée !");
        }

        if (carController == null)
        {
            Debug.LogError("Le script PrometeoCarcontroller n'est pas assigné !");
        }
    }

    void Update()
    {
        // Gestion du son du moteur (accélération)
        if (Input.GetKey(KeyCode.W))  // On suppose que W est utilisé pour accélérer
        {
            if (!isAccelerating)
            {
                engineSound.Play();  // Joue le son moteur si ce n'est pas déjà en cours
                isAccelerating = true;
            }
        }
        else
        {
            if (isAccelerating)
            {
                engineSound.Stop();  // Arrête le son moteur quand la touche est relâchée
                isAccelerating = false;
            }
        }

        // Gestion du son de dérive
        if (carController.isDrifting)  // La voiture commence à dériver
        {
            // Si on accélère aussi, joue les deux sons simultanément
            if (!driftingSound.isPlaying && isAccelerating)
            {
                driftingSound.Play();  // Joue le son de dérive
            }
        }
        else  // La voiture cesse de dériver
        {
            if (driftingSound.isPlaying)  // Vérifie si le son de dérive joue
            {
                driftingSound.Stop();  // Arrête le son de dérive
            }
        }
    }
}
