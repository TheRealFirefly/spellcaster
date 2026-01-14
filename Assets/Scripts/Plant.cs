using UnityEngine;
using System.Collections;

public class PlantEnemy : MonoBehaviour
{
    public GameObject projectilePrefab;   // Projektil-Prefab
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
                // Animation starten
                animator.SetBool("Attack", true);

                // Wartezeit während der Attack-Animation
                yield return new WaitForSeconds(0.5f);

                // Projektil schießen
                ShootAtPlayer();

                // Animation zurücksetzen
                animator.SetBool("Attack", false);

                // Warte bis zum nächsten Schuss
                float interval = GetShootInterval();
                yield return new WaitForSeconds(interval);
            }
            else
            {
                yield return null; // Sicherheit, falls player oder stats null sind
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
