using UnityEngine;

public class Blob : MonoBehaviour
{
    private float jumpForce;
    public float idleTime = 1.5f;
    public float jumpDuration = 0.25f;

    private Rigidbody2D rb;
    private Transform player;

    private bool isJumping = false;
    private float idleTimer;
    private EnemyStats enemyStats;

    public Animator animator;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        idleTimer = idleTime;
        rb.gravityScale = 0;
        jumpForce = GetJumpForce();
    }

    private void Update()
    {
        if (player == null) return;
        jumpForce = GetJumpForce();

        if (!isJumping)
        {
            idleTimer -= Time.deltaTime;

            if (idleTimer <= 0f)
            {
                StartCoroutine(Jump());
            }
        }
    }
    
    private System.Collections.IEnumerator Jump()
    {
        isJumping = true;
        animator.SetBool("IsJumping", true);

        Vector2 dir = (player.position - transform.position).normalized;

        rb.linearVelocity = dir * jumpForce;

        yield return new WaitForSeconds(jumpDuration);

        rb.linearVelocity = Vector2.zero;

        idleTimer = idleTime;
        isJumping = false;
        animator.SetBool("IsJumping", false);
    }

    private float GetJumpForce()
    {
        float baseForce = 6f;
        return baseForce * enemyStats.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(enemyStats.damage);
            }
        }
    }
}
