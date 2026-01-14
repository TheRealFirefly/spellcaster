using UnityEngine;

public class Projectile : MonoBehaviour
{      
    public float lifetime = 5f;     
    public enum ProjectileOwner { Player, Enemy}
    public ProjectileOwner owner;
    private Vector2 direction;
    private float damage;          
    private Rigidbody2D rb;
   
    public void Init(Vector2 dir, float dmg, float increase, ProjectileOwner projOwner, float speed)
    {
        direction = dir.normalized;
        damage = dmg;
        owner = projOwner;
        lifetime = lifetime + increase;

        rb = GetComponent<Rigidbody2D>();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        rb.linearVelocity = direction * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (owner == ProjectileOwner.Player && collision.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            if (enemy != null)
                enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (owner == ProjectileOwner.Enemy && collision.CompareTag("Player"))
        {
            PlayerStats player = collision.GetComponent<PlayerStats>();
            if (player != null)
                player.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

}

