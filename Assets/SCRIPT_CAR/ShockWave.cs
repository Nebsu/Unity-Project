using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ShockWave : MonoBehaviour
{
    [Header("Shockwave Settings")]
    public float shockwaveRadius = 2.5f;     // Rayon de la sphère (diamètre = 5 mètres)
    public float shockwaveForce = 500f;      // Force appliquée pour la propulsion
    public float shockwaveDuration = 0.5f;   // Durée d'existence de la sphère
    public KeyCode triggerKey = KeyCode.G;   // Touche pour déclencher l'effet

    [Header("Boost Settings")]
    public int boost = 10;                   // Valeur initiale du boost
    public int boostDecreaseRate = 1;        // Diminution du boost par seconde
    public int boostIncreaseRate = 2;        // Récupération du boost par seconde
    private float lastKeyPressTime;          // Temps depuis la dernière pression de G
    private bool isBoostRecovering = false;  // Indique si le boost est en récupération

    [Header("Fly Mode")]
    public bool fly = false;                 // Variable pour vérifier si le mode fly est actif

    [Header("Visual Settings")]
    public Color sphereColor = new Color(1f, 0f, 0f, 0.3f); // Couleur de la sphère (semi-transparente)

    private float nextShockwaveTime = 0f; // Temps minimum pour créer une nouvelle shockwave

    void Update()
    {
        // Vérifie si le mode fly est actif
        if (!fly)
            return; // Quitte la méthode si fly est faux

        // Gestion du boost quand la touche G est maintenue
        if (Input.GetKey(triggerKey))
        {
            // Réinitialise le délai de récupération
            lastKeyPressTime = Time.time;

            if (!isBoostRecovering)
            {
                StopCoroutine("RecoverBoost");
                isBoostRecovering = false;
            }

            // Crée des shockwaves si le délai est écoulé et boost est positif
            if (Time.time >= nextShockwaveTime && boost > 0)
            {
                CreateShockwave();
                nextShockwaveTime = Time.time + 0.2f; // Délai entre deux shockwaves

                // Diminue le boost
                StartCoroutine(DecreaseBoost());
            }
        }
        else
        {
            // Si G n'est pas maintenue et le délai de 3 secondes est écoulé, lance la récupération du boost
            if (Time.time - lastKeyPressTime >= 3f && !isBoostRecovering)
            {
                StartCoroutine(RecoverBoost());
            }
        }
    }

    public IEnumerator ActivateFlyForDuration(float duration)
    {
        fly = true; // Active le mode fly
        Debug.Log("Fly activé pour " + duration + " secondes !");

        yield return new WaitForSeconds(duration);

        fly = false; // Désactive le mode fly
        Debug.Log("Fly désactivé !");
    }

    private IEnumerator DecreaseBoost()
    {
        yield return new WaitForSeconds(1f); // Attente de 1 seconde
        boost -= boostDecreaseRate;          // Diminue le boost
        boost = Mathf.Max(boost, 0);         // Empêche le boost de descendre en dessous de 0
        Debug.Log("Boost actuel : " + boost);
    }

    private IEnumerator RecoverBoost()
    {
        isBoostRecovering = true;

        while (boost < 10)
        {
            yield return new WaitForSeconds(1f); // Attente de 1 seconde
            boost += boostIncreaseRate;          // Augmente le boost
            boost = Mathf.Min(boost, 10);        // Limite le boost à 10
            Debug.Log("Boost récupéré : " + boost);
        }

        isBoostRecovering = false;
    }

    private void CreateShockwave()
    {
        // Crée un objet vide pour la sphère de l'onde de choc
        GameObject shockwaveSphere = new GameObject("ShockWaveSphere");
        shockwaveSphere.transform.position = transform.position;

        // Ajoute un SphereCollider pour détecter les collisions
        SphereCollider shockwaveCollider = shockwaveSphere.AddComponent<SphereCollider>();
        shockwaveCollider.isTrigger = true;
        shockwaveCollider.radius = shockwaveRadius;

        // Ajoute un Rigidbody pour activer les triggers
        Rigidbody rb = shockwaveSphere.AddComponent<Rigidbody>();
        rb.isKinematic = true; // Pas de mouvement physique pour la sphère

        // Ajoute un effet visuel (optionnel)
        CreateVisualEffect(shockwaveSphere);

        // Ajoute le script de gestion des collisions
        ShockwaveEffect shockwaveEffect = shockwaveSphere.AddComponent<ShockwaveEffect>();
        shockwaveEffect.origin = this.gameObject; // Définit la voiture comme origine de l'onde
        shockwaveEffect.upwardForce = shockwaveForce;         // Force verticale forte
        shockwaveEffect.horizontalForce = shockwaveForce / 3; // Force horizontale plus faible

        // Détruit l'objet de l'onde après la durée définie
        Destroy(shockwaveSphere, shockwaveDuration);
    }

    private void CreateVisualEffect(GameObject shockwaveSphere)
    {
        // Ajoute un MeshRenderer pour la visualisation
        MeshRenderer renderer = shockwaveSphere.AddComponent<MeshRenderer>();
        MeshFilter filter = shockwaveSphere.AddComponent<MeshFilter>();
        filter.mesh = CreateSphereMesh();

        // Crée un matériau pour rendre la sphère transparente
        Material material = new Material(Shader.Find("Standard"));
        material.color = sphereColor;
        material.SetFloat("_Mode", 3); // Mode transparent
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        renderer.material = material;
    }

    private Mesh CreateSphereMesh()
    {
        // Crée une sphère temporaire pour récupérer son mesh
        GameObject tempSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Mesh mesh = tempSphere.GetComponent<MeshFilter>().mesh;

        // Détruit immédiatement la sphère temporaire pour éviter qu'elle reste dans la scène
        Destroy(tempSphere);

        return mesh;
    }
}
public class ShockwaveEffect : MonoBehaviour
{
    public GameObject origin;       // La voiture à l'origine de l'onde de choc
    public float upwardForce = 1000f;  // Force verticale appliquée
    public float horizontalForce = 300f; // Force horizontale appliquée

    private void OnTriggerEnter(Collider other)
    {
        // Récupère le Rigidbody et le NavMeshAgent dans les parents de l'objet touché
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();
        NavMeshAgent agent = other.GetComponentInParent<NavMeshAgent>();

        if (rb != null && other.gameObject != origin)
        {
            Debug.Log("Objet affecté par l'onde de choc : " + other.gameObject.name);

            // Si l'objet possède un NavMeshAgent, on le désactive temporairement
            if (agent != null)
            {
                StartCoroutine(DisableNavMeshTemporarily(agent, rb));
            }
            else
            {
                // Applique directement la force si aucun NavMeshAgent n'est présent
                ApplyShockwaveForce(rb);
            }
        }
    }

    private IEnumerator DisableNavMeshTemporarily(NavMeshAgent agent, Rigidbody rb)
    {
        agent.enabled = false; // Désactive le NavMeshAgent

        // Applique une force directionnelle
        ApplyShockwaveForce(rb);

        yield return new WaitForSeconds(3f); // Attente de 3 secondes

        agent.enabled = true; // Réactive le NavMeshAgent
        Debug.Log("NavMeshAgent réactivé sur : " + rb.gameObject.name);
    }

    private void ApplyShockwaveForce(Rigidbody rb)
    {
        // Calcule une force avec une poussée horizontale et verticale
        Vector3 horizontalDirection = (rb.transform.position - origin.transform.position).normalized;
        horizontalDirection.y = 0; // Annule la composante verticale pour la direction horizontale

        // Combine une forte poussée verticale avec une poussée horizontale
        Vector3 force = (horizontalDirection * horizontalForce) + (Vector3.up * upwardForce);

        rb.AddForce(force, ForceMode.Impulse);
        Debug.Log("Force appliquée : " + force);
    }
}
