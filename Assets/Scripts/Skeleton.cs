using System.Collections;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public EnemyStats stats;
    public Transform player;
    public Transform firePoint;
    public GameObject boomerangPrefab;

    private Rigidbody2D rb;
    private Animator animator;

    public float shootIntervalTicks;
    private float shootTimer = 0f;

    private bool isAttacking = false;

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
        animator = GetComponent<Animator>();

        shootIntervalTicks = stats.speed * 3f;
    }

    private void HandleTick()
    {
        if (isAttacking) return;

        shootTimer += 1f;

        if (shootTimer >= shootIntervalTicks)
        {
            shootTimer = 0f;
            StartCoroutine(ShootBoomerang());
        }
    }

    private IEnumerator ShootBoomerang()
    {
        isAttacking = true;

        animator.SetBool("Attack", true);

        yield return new WaitForSeconds(0.5f);

        if (player != null)
        {
            Vector2 dir = (player.position - transform.position).normalized;

            GameObject proj = Instantiate(
                boomerangPrefab,
                firePoint.position,
                Quaternion.identity
            );

            proj.GetComponent<Boomerang>().Init(
                dir,
                stats.damage,
                Projectile.ProjectileOwner.Enemy,
                transform,
                stats.speed * 4f
            );
        }

        animator.SetBool("Attack", false);

        yield return new WaitForSeconds(0.3f);

        isAttacking = false;
    }

    private void FixedUpdate()
    {
        if (player == null || isAttacking) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * stats.speed * Time.fixedDeltaTime);
    }
}
