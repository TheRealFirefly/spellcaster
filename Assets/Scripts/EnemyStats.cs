using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float HP;
    public float currentHP;
    public float damage;
    public float speed;
    public bool immuneToKnockback = false;
    public bool dead = false;
    private Coroutine hitFlashRoutine;
    private Coroutine critRoutine;
    private SpriteRenderer sr;
    private bool immune = false;

    public void InitializeStats(float baseHP, float hpMultiplier,
                                float baseDamage, float dmgMultiplier,
                                float speed, float spdMultiplier)                               
    {
        HP = Mathf.RoundToInt(baseHP * hpMultiplier);
        currentHP = HP;

        damage = Mathf.RoundToInt(baseDamage * dmgMultiplier);
        this.speed = Mathf.RoundToInt(speed * spdMultiplier);

        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (PlayerStats.instance.transform.position.x < transform.position.x)
            sr.flipX = false;
        else
            sr.flipX = true; 
    }

    public void TakeDamage(float amount)
    {
        if (dead || immune)
            return;

        bool isCrit = Random.value < PlayerStats.instance.critChance;

        if (isCrit)
        {
            amount *= PlayerStats.instance.critDMG;

            if (hitFlashRoutine != null)
            {
                StopCoroutine(hitFlashRoutine);
                hitFlashRoutine = null;
            }
            if (critRoutine != null)
                StopCoroutine(critRoutine);

            critRoutine = StartCoroutine(CritFlash());
        }
        else
        {

            if (hitFlashRoutine != null)
                StopCoroutine(hitFlashRoutine);

            hitFlashRoutine = StartCoroutine(HitFlash());
        }

        currentHP -= amount;

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

    IEnumerator CritFlash()
    {
        if (sr == null) yield break;

        sr.color = Color.gray4;

        yield return new WaitForSecondsRealtime(0.12f);

        sr.color = Color.darkOrange;

        yield return new WaitForSecondsRealtime(0.12f);

        sr.color = Color.red;

        yield return new WaitForSecondsRealtime(0.12f);
        sr.color = Color.white;

        hitFlashRoutine = null;
    }

    void Die()
    {
        if (dead)
            return;

        dead = true;

        WaveManager.instance.NotifyEnemyKilled();
        Destroy(gameObject);
    }

    public void ApplySlow(float amount, float duration)
    {
        StartCoroutine(SlowCoroutine(amount, duration));
    }

    IEnumerator SlowCoroutine(float amount, float duration)
    {
        speed *= amount;
        sr.color = Color.cyan;
        yield return new WaitForSeconds(duration);
        sr.color = Color.white;
        speed /= amount;
    }
}
