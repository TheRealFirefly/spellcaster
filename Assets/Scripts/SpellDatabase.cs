using UnityEngine;

public class SpellDatabase : MonoBehaviour
{
    public static SpellDatabase instance;

    public SpellBase[] spells;

    private void Awake()
    {
        instance = this;
    }

    public SpellBase LearnSpell(float index)
    {
        if (spells[(int)index] != null)
        {
            return spells[(int)index];
        }
        return null;
    }
}
