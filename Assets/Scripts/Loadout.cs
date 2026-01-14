using UnityEngine;

public class Loadout : MonoBehaviour
{
    public Transform spellArea;
    public GameObject loadoutSlot;
    public LoadoutSpellSlot[] slots;

    private void Start()
    {
        SpellBase[] allSpells = SpellDatabase.instance.spells;
        slots = new LoadoutSpellSlot[allSpells.Length];

        for (int i = 0; i < allSpells.Length; i++)
        {
            GameObject slotGO = Instantiate(loadoutSlot, spellArea);
            LoadoutSpellSlot slot = slotGO.GetComponent<LoadoutSpellSlot>();

            slot.SetSpell(allSpells[i]);
            slots[i] = slot;
        }
    }

}
