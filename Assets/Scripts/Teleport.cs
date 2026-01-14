using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Teleport")]
public class Teleport : SpellBase
{
    public GameObject teleportPrefab;

    public float radius;

    public override void Cast(Vector3 position)
    {
        Transform player = PlayerStats.instance.transform;

        // --- Spieler teleportieren ---
        player.position = position;

        // --- Teleport-Effekt instanziieren ---
        if (teleportPrefab != null)
        {
            GameObject effect = Instantiate(teleportPrefab, position, Quaternion.identity);

            // Radius skalieren wie beim Explosion-Spell
            float scaleRadius = radius * (1f + PlayerStats.instance.areaMult);
            float diameter = scaleRadius * 2f;

            SpriteRenderer sr = effect.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                float spriteWorldSize = sr.sprite.bounds.size.x;
                float scaleFactor = diameter / spriteWorldSize;
                effect.transform.localScale = Vector3.one * scaleFactor;
            }
        }

        // --- Gegner im Radius treffen ---
        float hitRadius = radius * (1f + PlayerStats.instance.areaMult);
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, hitRadius);
        HashSet<EnemyStats> hitEnemies = new HashSet<EnemyStats>();

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent<EnemyStats>(out var enemy)) continue;
            if (hitEnemies.Contains(enemy)) continue;

            hitEnemies.Add(enemy);

            // Schaden anwenden
            enemy.TakeDamage(damageMult * PlayerStats.instance.damage);
            
        }
    }
}
