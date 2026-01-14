using UnityEngine;
using System.Collections;

public class PlantEnemy : MonoBehaviour
{
    public GameObject projectilePrefab;
    private Transform player;
    private EnemyStats enemyStats;
    private Animator animator;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            if (player != null && enemyStats != null)
            {
                animator.SetBool("Attack", true);

                yield return new WaitForSeconds(0.5f);

                ShootAtPlayer();

                animator.SetBool("Attack", false);

                float interval = GetShootInterval();
                yield return new WaitForSeconds(interval);
            }
            else
            {
                yield return null;
            }
        }
    }

    private float GetShootInterval()
    {
        return Mathf.Max(0.1f, 1f / Mathf.Sqrt(enemyStats.speed));
    }

    private void ShootAtPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectileScript = proj.GetComponent<Projectile>();
        projectileScript.Init(direction, enemyStats.damage, 0, Projectile.ProjectileOwner.Enemy, enemyStats.speed + 3);
    }

}
