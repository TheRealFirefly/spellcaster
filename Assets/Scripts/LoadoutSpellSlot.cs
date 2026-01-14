using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadoutSpellSlot : MonoBehaviour
{
    public TMP_Text spellName;
    public Image icon;
    public Image chosenBorder;
    public Button chooseButton;
    public SpellManager spells;
    public SpellBase storedSpell;
    public TooltipTrigger trigger;

    public static List<LoadoutSpellSlot> allSlots = new();

    private void Awake()
    {
        allSlots.Add(this);
        spells = FindFirstObjectByType<SpellManager>();
    }

    private void OnDestroy()
    {
        allSlots.Remove(this);
    }
    public void SetSpell(SpellBase spell)
    {
        if (spell == null) return;
        storedSpell = spell;
        spellName.enabled = true;
        spellName.text = spell.name;
        icon.enabled = true;
        icon.sprite = spell.icon;
        chosenBorder.enabled = false;
        trigger.tooltipText = spell.description + "\n Mana: " + spell.manaCost + "\n cooldown: " + spell.cooldown;
    }

    public void ChooseSpell()
    {
        if (storedSpell == null) return;

        // Schon aktiv?
        for (int i = 0; i < spells.activeSpells.Length; i++)
        {
            if (spells.activeSpells[i] == storedSpell)
                return;
        }

        // Freier Slot
        for (int i = 0; i < spells.activeSpells.Length; i++)
        {
            if (spells.activeSpells[i] == null)
            {
                spells.activeSpells[i] = storedSpell;
                chosenBorder.enabled = true;
                return;
            }
        }

        // ❗ Kein Slot frei → ältesten entfernen
        SpellBase removed = spells.activeSpells[0];

        // UI vom entfernten Spell updaten
        LoadoutSpellSlot oldSlot = FindSlotBySpell(removed);
        if (oldSlot != null)
            oldSlot.Unchoose();

        // Array verschieben
        spells.activeSpells[0] = spells.activeSpells[1];
        spells.activeSpells[1] = storedSpell;

        chosenBorder.enabled = true;
    }

    public void Unchoose()
    {
        chosenBorder.enabled = false;
    }

    public static LoadoutSpellSlot FindSlotBySpell(SpellBase spell)
    {
        foreach (var slot in allSlots)
        {
            if (slot.storedSpell == spell)
                return slot;
        }
        return null;
    }

}
