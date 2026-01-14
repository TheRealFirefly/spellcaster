using UnityEngine;

[CreateAssetMenu(fileName = "SpellBase", menuName = "Scriptable Objects/SpellBase")]
public abstract class SpellBase : ScriptableObject
{
    public string SpellName;
    public Sprite icon;
    public float cooldown;
    public float baseManaCost;
    public virtual float manaCost => 
        baseManaCost;
    public float damageMult;
    public string description;
    public abstract void Cast(Vector3 position);

}
