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

    private void Start()
    {
        // Ensure particles don't play on load. They should only play when moving or on explosion.
        if (moveParticles != null)
        {
            moveParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        if (explosionParticles != null)
        {
            explosionParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            
            // Destroy the player object
            Destroy(gameObject);

             if (explosionParticles != null)
            {
                ParticleSystem explosionClone = Instantiate(explosionParticles, transform.position, Quaternion.identity);
                explosionClone.Play();

                // Try to compute an appropriate lifetime for automatic destruction of the clone.
                var main = explosionClone.main;
                float lifetime = main.duration;
                // Add startLifetime (use constantMax to be safe with MinMaxCurve)
                lifetime += main.startLifetime.constantMax;
                Destroy(explosionClone.gameObject, lifetime + 0.1f);
            }

            // Stop movement particles immediately so they don't continue emitting.
            if (moveParticles != null && moveParticles.isPlaying)
            {
                moveParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
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