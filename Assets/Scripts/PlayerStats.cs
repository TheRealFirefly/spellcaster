using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    public float maxHP = 250;
    public float currentHP;
    public float hpRegen = 1;
    public float maxMana = 100;
    public float currentMana;
    public float manaRegen = 3;
    public float money = 0;
    public float damage = 10;
    public float speed = 3f;
    public float critChance = 0;
    public float critDMG = 1.5f;
    public float armor = 0;
    public float areaMult = 1;
    public float cooldownRed = 0;
    public float projecRange = 0;
    public float projecSpeed = 0;
    public float castSpeed = 1.66f;

    public bool immune = false;

    public System.Action OnDeath;
    public static PlayerStats instance;
    private SpellManager spells;

    public ItemData consumable = null;
    public HPBarUI hpBar;
    public ManaBarUI manaBar;

    private Coroutine hitFlashRoutine;
    private SpriteRenderer sr;

    private void Awake()
    {
        instance = this;
        sr = GetComponent<SpriteRenderer>();
        
    }

    private void Update()
    {
        hpBar.SetHP(currentHP, maxHP);
        manaBar.SetHP(currentMana, maxMana);
    }
    void Start()
    {
        currentHP = maxHP;
        currentMana = maxMana;
        spells = GetComponent<SpellManager>();

    }

    private void OnEnable()
    {
        if (TickManager.instance != null)
        {
            TickManager.instance.OnTick += Tick;
        }
    }

    private void OnDisable()
    {
        if (TickManager.instance != null)
        {
            TickManager.instance.OnTick -= Tick;
        }
    }

    private void Tick()
    {
        RegenerateHP();
        RegenerateMana();
        
    }

    private void RegenerateHP()
    {
        if (currentHP < maxHP)
        { 
            currentHP = Mathf.Min(currentHP + hpRegen, maxHP);
        }
    }

    private void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana = Mathf.Min(currentMana + manaRegen, maxMana);
        }
    }

    public void TakeDamage(float amount)
    {
        if (immune)
            return;

        currentHP -= amount;

        if (hitFlashRoutine != null)
        {
            StopCoroutine(HitFlash());
        }

        hitFlashRoutine = StartCoroutine(HitFlash());

        if (currentHP <= 0)
        {
            Die();
        }
    }

    IEnumerator HitFlash()
    {
        if (sr == null) yield break;

        sr.color = Color.red;

        yield return new WaitForSecondsRealtime(0.08f);

        sr.color = Color.white;

        hitFlashRoutine = null;
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

    public void AddMoney(float amount)
    {
        money += amount;
    }

    public void SpendMoney(float amount)
    { 
        money -= amount;
    }

    public void interest()
    {
        switch(money)
        {
            
            case >= 50:
                AddMoney(5);
                break;

            case >= 40:
                AddMoney(4);
                break;

            case >= 30:
                AddMoney(3);
                break;

            case >= 20:
                AddMoney(2);
                break;

            case >= 10:
                AddMoney(1);
                break;
        }

    }


    public void ApplyItem(ItemData item)
    {
        switch (item.type)
        {
            case BuffType.Armor: armor += item.buffValue; break;
            case BuffType.Area: areaMult += item.buffValue; break;
            case BuffType.HP: maxHP += Mathf.RoundToInt(maxHP * item.buffValue); break;
            case BuffType.Damage: damage += damage * item.buffValue; break;
            case BuffType.Mana: maxMana += Mathf.RoundToInt(maxMana * item.buffValue); break;
            case BuffType.CooldownReduction: cooldownRed += item.buffValue * (1f - cooldownRed); break;
            case BuffType.CritChance: critChance += item.buffValue; break;
            case BuffType.CritDamage: critDMG += item.buffValue; break;
            case BuffType.HPRegen: hpRegen += item.buffValue; break;
            case BuffType.ManaRegen: manaRegen += item.buffValue; break;
            case BuffType.MovementSpeed: speed += speed * item.buffValue; break;
            case BuffType.CastSpeed: castSpeed *= 1f - item.buffValue; break;

            case BuffType.Spell:
                {
                    SpellBase newSpell = SpellDatabase.instance.LearnSpell(item.buffValue);

                    bool alreadyHas = false;
                    for (int i = 0; i < spells.activeSpells.Length; i++)
                    {
                        if (spells.activeSpells[i] == newSpell)
                        {
                            alreadyHas = true;
                            break;
                        }
                    }
                    if (alreadyHas) break;

                    bool placed = false;
                    for (int i = 0; i < spells.activeSpells.Length; i++)
                    {
                        if (spells.activeSpells[i] == null)
                        {
                            spells.activeSpells[i] = newSpell;
                            placed = true;
                            break;
                        }
                    }

                    if (!placed)
                    {
                        spells.activeSpells[0] = spells.activeSpells[1]; 
                        spells.activeSpells[1] = newSpell;
                    }
                    break;
                }
        }
    }

    public void ConsumeItem(ItemData item)
    {
        if (item.type == BuffType.HP)
        {
            int healAmount = Mathf.RoundToInt(maxHP * item.buffValue);
            currentHP = Mathf.Min(currentHP + healAmount, maxHP);
        }
        else
        {
            int manaAmount = Mathf.RoundToInt(maxMana * item.buffValue);
            currentMana = Mathf.Min(currentMana + manaAmount, maxMana);
        }

        consumable = null;
    }

    public bool HasSpell(SpellBase spellCheck)
    {
        foreach(var spell in spells.activeSpells)
        {
            if (spell != null && spell == spellCheck) return true;
        }
        return false;
    }
}
