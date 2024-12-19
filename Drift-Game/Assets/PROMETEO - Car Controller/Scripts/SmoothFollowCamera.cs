using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    [Header("Paramètres de la caméra")]
    public Transform target;  // La voiture à suivre (drag and drop dans l'inspecteur)
    public Vector3 offset = new Vector3(0f, 5f, -10f);  // Position relative par défaut
    public float positionSmoothSpeed = 5f;  // Vitesse de lissage pour la position
    public float rotationSmoothSpeed = 3f;  // Vitesse de lissage pour la rotation

    [Header("Paramètres d'évitement des obstacles")]
    public float cameraCollisionRadius = 0.5f;  // Rayon pour détecter les collisions
    public float collisionOffset = 0.2f;  // Distance minimale à conserver par rapport aux obstacles
    public LayerMask collisionLayers;  // Les couches à considérer comme obstacles

    private Vector3 currentVelocity;  // Pour SmoothDamp sur la position

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("La référence de la voiture n'est pas assignée !");
            return;
        }

        // 1. Calcul de la position cible pour la caméra
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        // 2. Vérification des obstacles avec un Raycast
        Vector3 directionToTarget = desiredPosition - target.position;
        RaycastHit hit;

        if (Physics.SphereCast(target.position, cameraCollisionRadius, directionToTarget.normalized, out hit, directionToTarget.magnitude, collisionLayers))
        {
            // Si un obstacle est détecté, ajuster la position pour éviter la collision
            desiredPosition = hit.point + hit.normal * collisionOffset;
        }

        // 3. Lissage de la position de la caméra
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, 1f / positionSmoothSpeed);

        // 4. Lissage de la rotation pour toujours regarder la voiture
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSmoothSpeed);
    }
}