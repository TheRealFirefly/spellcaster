using UnityEngine;

[CreateAssetMenu(menuName = "Spells/LifeDrop")]
public class LifeDrop : SpellBase
{
    public GameObject prefab;
    public override float manaCost => PlayerStats.instance.currentMana;
    public override void Cast(Vector3 position)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector3 center = player.transform.position;

        GameObject explosion = Instantiate(prefab, center, Quaternion.identity);
        PlayerStats.instance.currentHP =
            Mathf.Min(
                PlayerStats.instance.currentHP + PlayerStats.instance.currentMana * 2,
                PlayerStats.instance.maxHP);
    }
}
