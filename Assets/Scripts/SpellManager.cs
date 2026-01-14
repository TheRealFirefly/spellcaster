using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellManager : MonoBehaviour
{
    public SpellBase[] activeSpells = new SpellBase[2];
    public float[] cooldownTimers = new float[2];
    private bool subscribed = false;
    private Camera mainCam;
    private Animator animator;
    private PlayerController controller;
    private bool isCasting;

    private void Awake()
    {
        mainCam = Camera.main;
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
    }
    void Start()
    {
        if (!subscribed && TickManager.instance != null)
        {
            TickManager.instance.OnTick += Tick;
            subscribed = true;
        }
    }

    private void OnDestroy()
    {
        if (subscribed && TickManager.instance != null)
        {
            TickManager.instance.OnTick -= Tick;
            subscribed = false;
        }
    }


    private void Tick()
    {
        for (int i = 0; i < activeSpells.Length; i++)
        {
            if (cooldownTimers[i] > 0)
                cooldownTimers[i] -= 1f;
        }
        
    }

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame) TryCast(0);
        if (Keyboard.current.eKey.wasPressedThisFrame) TryCast(1);
    }

    void TryCast(int index)
    {
        if (index < 0 || index >= activeSpells.Length) return;
        if (activeSpells[index] == null) return;
        if (cooldownTimers[index] > 0f) return;
        if (Mouse.current == null || Camera.main == null) return;
        if (PlayerStats.instance.currentMana < activeSpells[index].manaCost) return;

        StartCoroutine(CastRoutine(index));
    }

    IEnumerator CastRoutine(int index)
    {
        if (isCasting) yield break;
        isCasting = true;

        controller.paused = true;
        PlayerStats.instance.immune = true;

        Rigidbody2D rb = controller.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        animator.SetTrigger("Cast");

        float baseAnimLength = 1.5f; 
        animator.speed = baseAnimLength / PlayerStats.instance.castSpeed;

        yield return new WaitForSeconds(PlayerStats.instance.castSpeed);

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 castPos = mainCam.ScreenToWorldPoint(mousePos);
        castPos.z = 0f;

        activeSpells[index].Cast(castPos);

        cooldownTimers[index] =
            activeSpells[index].cooldown
            - activeSpells[index].cooldown * PlayerStats.instance.cooldownRed;

        PlayerStats.instance.currentMana -= activeSpells[index].manaCost;

        animator.speed = 1f;
        controller.paused = false;
        PlayerStats.instance.immune = false;
        isCasting = false;
        animator.SetTrigger("CastDone");
    }

}
