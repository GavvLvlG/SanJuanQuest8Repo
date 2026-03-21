using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem particleSystem2D;

    [Header("Settings")]
    [Tooltip("If true, the particle effect will destroy itself after finishing.")]
    public bool autoDestroy = true;

    [Tooltip("Delay before destroying the GameObject after particles stop.")]
    public float destroyDelay = 0.5f;

    void Awake()
    {
        // Get the ParticleSystem component
        particleSystem2D = GetComponent<ParticleSystem>();

        if (particleSystem2D == null)
        {
            Debug.LogError("No ParticleSystem found on this GameObject.");
        }
    }

    void Update()
    {
        // Auto-destroy when finished
        if (autoDestroy && particleSystem2D != null && !particleSystem2D.IsAlive())
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    /// <summary>
    /// Plays the particle effect.
    /// </summary>
    public void PlayEffect()
    {
        if (particleSystem2D != null)
        {
            particleSystem2D.Play();
        }
    }

    /// <summary>
    /// Stops the particle effect.
    /// </summary>
    public void StopEffect()
    {
        if (particleSystem2D != null)
        {
            particleSystem2D.Stop();
        }
    }

    /// <summary>
    /// Plays the effect at a specific position.
    /// </summary>
    public void PlayAtPosition(Vector3 position)
    {
        transform.position = position;
        PlayEffect();
    }
}
