using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Ice Circle")]
public class IceCircleSpell : SpellBase
{
    public GameObject iceAreaPrefab;
    public float radius;
    public float slowAmount;
    public float slowDuration;

    public override void Cast(Vector3 position)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector3 center = player.transform.position;

        GameObject explosion = Instantiate(iceAreaPrefab, center, Quaternion.identity);

        // EIN finaler Radius für alles
        float finalRadius = radius * (1f + PlayerStats.instance.areaMult);
        float diameter = finalRadius * 2f;

        // Sprite korrekt skalieren
        SpriteRenderer sr = explosion.GetComponent<SpriteRenderer>();
        float spriteWorldSize = sr.sprite.bounds.size.x;
        float scaleFactor = diameter / spriteWorldSize;
        explosion.transform.localScale = Vector3.one * scaleFactor;

        // Hitbox nutzt EXAKT denselben Radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, finalRadius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<EnemyStats>(out var enemy))
            {
                enemy.TakeDamage(damageMult * PlayerStats.instance.damage);
                enemy.ApplySlow(slowAmount, slowDuration);
            }
        }
    }
}

