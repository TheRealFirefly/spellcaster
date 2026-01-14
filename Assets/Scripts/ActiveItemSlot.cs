using JetBrains.Annotations;
using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveItemSlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text key;
    public TMP_Text cooldown;
    public GameObject gray;
    public TooltipTrigger trigger;

    private SpellBase currentSpell;
    private int spellIndex;
    private SpellManager spellManager;

    public void setSpell(SpellBase spell, int index, SpellManager manager)
    {
        currentSpell = spell;
        spellIndex = index;
        spellManager = manager;

        if (spell == null)
        {
            icon.enabled = false;
            gray.SetActive(true);
            cooldown.text = "";
            return;
        }

        icon.enabled = true;
        icon.sprite = spell.icon;
        gray.SetActive(false);
        trigger.tooltipText = spell.description + "\n Mana: " + spell.manaCost + "\n Cooldown: " + spell.cooldown;

        key.enabled = true;
        key.text = index == 0 ? "Q" : "E";
    }

    public void SetConsumable(ItemData item)
    {
        if(item == null)
        {
            icon.enabled = false;
            gray.SetActive(true);
            cooldown.text = "";
            return;
        }

        icon.enabled = true;
        icon.sprite = item.icon;
        gray.SetActive(false);

        key.enabled = true;
        key.text = "F";
    }

    public void UpdateUI(int index)
    {
        if (currentSpell != null)
        { 
            trigger.tooltipText = currentSpell.description + "\n Mana: " + currentSpell.manaCost + "\n Cooldown: " + currentSpell.cooldown; 
        }
        if (currentSpell == null | spellManager == null)
            return;

        float cd = spellManager.cooldownTimers[index];

        if (cd > 0)
        {
            gray.SetActive(true);
            cooldown.text = Mathf.CeilToInt(cd).ToString();
        }
        else
        {
            gray.SetActive(false);
            cooldown.text = "";
        }
    }
}
