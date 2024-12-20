using System.Collections;
using UnityEngine;

public class SimpleFPSController : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public float gravityDelay = 0.2f;

    private CharacterController characterController;
    private Vector3 velocity;
    private Transform cameraTransform;
    private float verticalRotation = 0f;
    public bool isJumping;
    public bool grounded;
    

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        grounded = characterController.isGrounded;
        Move();
        Look();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * speed * Time.deltaTime);

        if (characterController.isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = -2f; // Reset gravity only when touching ground
                isJumping = false;
            }

            if (Input.GetButton("Jump") && !isJumping)
            {
                StartCoroutine(ApplyJump());
            }
        }
        else if (!isJumping)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    IEnumerator ApplyJump()
    {
        isJumping = true;
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        yield return new WaitForSeconds(gravityDelay);
        isJumping = false;
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
