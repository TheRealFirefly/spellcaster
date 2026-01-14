using UnityEngine;

public class OrbEnemy : MonoBehaviour
{
    public EnemyStats stats;             // HP, Damage, Speed
    public Transform player;
    public Transform firePoint;
    public GameObject projectilePrefab;
    private Rigidbody2D rb;

    public int spreadCount = 3;
    public float spreadAngle = 30f;

    private float shootIntervalTicks;   // abhängig von stats.speed
    private float shootTimer = 0f;

    private void OnEnable()
    {
        TickManager.instance.OnTick += HandleTick;
    }

    private void OnDisable()
    {
        TickManager.instance.OnTick -= HandleTick;
    }

    private void Start()
    {
        player = PlayerStats.instance.transform;
        rb = GetComponent<Rigidbody2D>();
        UpdateShootInterval();
    }

    private void UpdateShootInterval()
    {
        // shootInterval abhängig von speed: schneller -> häufiger schießen
        // Minimum 0.5 Ticks, Maximum 3 Ticks z.B.
        shootIntervalTicks = Mathf.Clamp(3f - stats.speed, 0.5f, 3f);
    }

    private void HandleTick()
    {
        shootTimer += 1f; // 1 Tick

        if (shootTimer >= shootIntervalTicks)
        {
            ShootSpread(spreadCount, spreadAngle);
            shootTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // Bewegung abhängig von speed
        Vector2 dir = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * stats.speed * Time.fixedDeltaTime);
    }

    private void ShootSpread(int count, float angle)
    {
        float startAngle = -angle / 2f;
        float step = count > 1 ? angle / (count - 1) : 0;

        for (int i = 0; i < count; i++)
        {
            float currentAngle = startAngle + step * i;
            Vector2 shootDir = Quaternion.Euler(0, 0, currentAngle) * (player.position - transform.position).normalized;

            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            // Projektilgeschwindigkeit proportional zur stats.speed
            float projectileSpeed = 2f * stats.speed; // Basisgeschwindigkeit * Orb-Speed
            proj.GetComponent<Projectile>().Init(shootDir, stats.damage, 5f, Projectile.ProjectileOwner.Enemy, projectileSpeed);
        }
    }
}
