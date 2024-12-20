using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    [Header("Paramètres de la caméra")]
    public Transform target;  // La voiture à suivre (drag and drop dans l'inspecteur)
    public Vector3 offset = new Vector3(0f, 5f, -10f);  // Position relative par défaut
    public Vector3 reverseOffset = new Vector3(0f, 5f, 10f);  // Position pour la caméra arrière
    public float positionSmoothSpeed = 5f;  // Vitesse de lissage pour la position
    public float rotationSmoothSpeed = 10f;  // Vitesse de lissage pour la rotation

    [Header("Paramètres d'évitement des obstacles")]
    public float cameraCollisionRadius = 0.5f;  // Rayon pour détecter les collisions
    public float collisionOffset = 0.2f;  // Distance minimale à conserver par rapport aux obstacles
    public LayerMask collisionLayers;  // Les couches à considérer comme obstacles

    [Header("Entrée utilisateur")]
    public KeyCode reverseKey = KeyCode.LeftShift;  // Touche pour la caméra arrière (Shift)

    private Vector3 currentVelocity;  // Pour SmoothDamp sur la position
    private bool isReversing = false;  // État de la marche arrière
    private bool lastReversingState = false; // Pour détecter les changements d'état

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("La référence de la voiture n'est pas assignée !");
            return;
        }

        // Détecte si la touche pour activer la caméra arrière est enfoncée
        isReversing = Input.GetKey(reverseKey);

        // Si l'état de marche arrière a changé, mettre à jour instantanément
        if (isReversing != lastReversingState)
        {
            UpdateCameraInstantly();
        }
        lastReversingState = isReversing;

        // 1. Calcul de la position cible pour la caméra
        Vector3 desiredOffset = isReversing ? reverseOffset : offset; // Utilise l'offset arrière si la touche est enfoncée
        Vector3 desiredPosition = target.position + target.TransformDirection(desiredOffset);

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
        Quaternion desiredRotation = isReversing
            ? Quaternion.LookRotation(-target.forward, Vector3.up) // Caméra arrière
            : Quaternion.LookRotation(target.position - transform.position, Vector3.up); // Caméra normale

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSmoothSpeed);
    }

    /// <summary>
    /// Met à jour immédiatement la position et la rotation de la caméra lors du changement d'état.
    /// </summary>
    private void UpdateCameraInstantly()
    {
        Vector3 desiredOffset = isReversing ? reverseOffset : offset;
        Vector3 desiredPosition = target.position + target.TransformDirection(desiredOffset);

        // Appliquer directement la position
        transform.position = desiredPosition;

        // Appliquer directement la rotation
        Quaternion desiredRotation = isReversing
            ? Quaternion.LookRotation(-target.forward, Vector3.up)
            : Quaternion.LookRotation(target.position - transform.position, Vector3.up);

        transform.rotation = desiredRotation;
    }
}