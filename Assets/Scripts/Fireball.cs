using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Fireball")]
public class FireballSpell : SpellBase
{
    public GameObject explosionPrefab;
    public float radius;

    public override void Cast(Vector3 position)
    {
        GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);

        float scaleRadius = radius * (1f + PlayerStats.instance.areaMult);
        float diameter = scaleRadius * 2f;
        SpriteRenderer sr = explosion.GetComponent<SpriteRenderer>();
        float spriteWorldSize = sr.sprite.bounds.size.x; 

        float scaleFactor = diameter / spriteWorldSize;
        explosion.transform.localScale = Vector3.one * scaleFactor;
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius + radius*PlayerStats.instance.areaMult);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<EnemyStats>(out var enemy))
            {
                enemy.TakeDamage(damageMult * PlayerStats.instance.damage);
            }
        }
    }
}