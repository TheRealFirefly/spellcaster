using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float speed;
    public float damage;
    public Projectile.ProjectileOwner owner;
    public float maxDistance = 5f; 
    private Vector2 startPos;
    private Vector2 direction;
    private bool returning = false;
    private Transform ownerTransform;

    public void Init(Vector2 dir, float dmg, Projectile.ProjectileOwner projOwner, Transform ownerTransform, float speed)
    {
        startPos = transform.position;
        direction = dir.normalized;
        damage = dmg;
        owner = projOwner;
        this.ownerTransform = ownerTransform;
        this.speed = speed;
    }

    private void FixedUpdate()
    {
        if (!returning)
        {
            transform.position += (Vector3)(direction * speed * Time.fixedDeltaTime);

            if (Vector2.Distance(startPos, transform.position) >= maxDistance)
            {
                returning = true;
            }
        }
        else
        {
            if (ownerTransform == null)
            {
                Destroy(gameObject); // Owner weg -> Bumerang zerstören
                return;
            }

            // Zurück zum Owner
            Vector2 returnDir = ((Vector2)ownerTransform.position - (Vector2)transform.position).normalized;
            transform.position += (Vector3)(returnDir * speed * Time.fixedDeltaTime);

            if (Vector2.Distance(transform.position, ownerTransform.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (owner == Projectile.ProjectileOwner.Enemy && collision.CompareTag("Player"))
        {
            PlayerStats player = collision.GetComponent<PlayerStats>();
            if (player != null)
                player.TakeDamage(damage);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}