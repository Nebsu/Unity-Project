using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    [Header("Param�tres de la cam�ra")]
    public Transform target;  // La voiture � suivre (drag and drop dans l'inspecteur)
    public Vector3 offset = new Vector3(0f, 5f, -10f);  // Position relative par d�faut
    public float positionSmoothSpeed = 5f;  // Vitesse de lissage pour la position
    public float rotationSmoothSpeed = 3f;  // Vitesse de lissage pour la rotation

    [Header("Param�tres d'�vitement des obstacles")]
    public float cameraCollisionRadius = 0.5f;  // Rayon pour d�tecter les collisions
    public float collisionOffset = 0.2f;  // Distance minimale � conserver par rapport aux obstacles
    public LayerMask collisionLayers;  // Les couches � consid�rer comme obstacles

    private Vector3 currentVelocity;  // Pour SmoothDamp sur la position

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("La r�f�rence de la voiture n'est pas assign�e !");
            return;
        }

        // 1. Calcul de la position cible pour la cam�ra
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        // 2. V�rification des obstacles avec un Raycast
        Vector3 directionToTarget = desiredPosition - target.position;
        RaycastHit hit;

        if (Physics.SphereCast(target.position, cameraCollisionRadius, directionToTarget.normalized, out hit, directionToTarget.magnitude, collisionLayers))
        {
            // Si un obstacle est d�tect�, ajuster la position pour �viter la collision
            desiredPosition = hit.point + hit.normal * collisionOffset;
        }

        // 3. Lissage de la position de la cam�ra
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, 1f / positionSmoothSpeed);

        // 4. Lissage de la rotation pour toujours regarder la voiture
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSmoothSpeed);
    }
}