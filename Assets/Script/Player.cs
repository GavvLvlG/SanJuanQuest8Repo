using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 5f;

    private InputAction moveAction;
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    [Header("Particles")]
    public ParticleSystem moveParticles; // Assign this in the Inspector
    public ParticleSystem explosionParticles; // Assign this in the Inspector

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();

        if (playerInput != null)
        {
            moveAction = playerInput.actions.FindAction("Move");
            if (moveAction == null)
                Debug.LogError("Move action not found! Check your Input Actions.");
        }
        else
        {
            Debug.LogError("PlayerInput component missing!");
        }
    }

    void FixedUpdate()
    {
        if (moveAction == null)
            return; // Prevent NullReferenceException

        Vector2 direction = moveAction.ReadValue<Vector2>();
        Move(direction);
        HandleParticles(direction);
    }

    private void Move(Vector2 direction)
    {
        Vector2 normalizedDirection = direction.normalized;
        Vector2 velocity = normalizedDirection * speed;
        rb.linearVelocity = velocity; // Correct Rigidbody2D property
    }

    private void HandleParticles(Vector2 direction)
    {
        if (moveParticles == null)
            return;

        if (direction.magnitude > 0.1f) // Player is moving
        {
            if (!moveParticles.isPlaying)
                moveParticles.Play();
        }
        else
        {
            if (moveParticles.isPlaying)
                moveParticles.Stop();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Play explosion particles
            if (explosionParticles != null)
            {
                explosionParticles.transform.position = transform.position; // Position explosion at player
                explosionParticles.Play();
            }

            // Destroy the player object
            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Stay");
    }
}