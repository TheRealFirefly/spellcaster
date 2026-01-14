using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyStats))]
public class Wolf : MonoBehaviour
{
    private Rigidbody2D rb;
    private EnemyStats stats;
    private Transform player;

    private bool touchingPlayer = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        player = PlayerStats.instance.transform;
    }

    private void OnEnable()
    {
        TickManager.instance.OnTick += DealDamage;
    }

    private void OnDisable()
    {
        if (TickManager.instance != null)
            TickManager.instance.OnTick -= DealDamage;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * stats.speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            touchingPlayer = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            touchingPlayer = false;
    }

    private void DealDamage()
    {
        if (!touchingPlayer) return;

        PlayerStats.instance.TakeDamage(stats.damage);
    }
}
