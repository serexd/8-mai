using UnityEngine;

public class PlatformAbility : MonoBehaviour
{
    [Header("Platform Settings")]
    public GameObject platformPrefab;
    public float platformDuration = 2f;
    public Vector2 spawnOffset = new Vector2(0f, -1f);

    [Header("Input")]
    public KeyCode abilityKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Cooldown")]
    public float cooldown = 1.5f;

    private bool canUse = true;
    private float cooldownTimer = 0f;

    void Update()
    {
        // Cooldown timer
        if (!canUse)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f && IsGrounded())
            {
                canUse = true;
            }
        }

        // Input
        if (Input.GetKeyDown(abilityKey) && canUse && !IsGrounded())
        {
            SpawnPlatform();
            canUse = false;
            cooldownTimer = cooldown;
        }
    }

    void SpawnPlatform()
    {
        Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;
        GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        Destroy(platform, platformDuration);
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
