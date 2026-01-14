using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;
    private Rigidbody2D m_Rigidbody;
    private Vector2 inputDirection;

    private float attackCooldown = 1f;
    private float lastAttackTime = 0f;

    public GameObject projectilePrefab;
    public Transform firePoint;

    private PlayerStats m_PlayerStats;
    public Boolean paused = false;

    public Animator animator;

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        m_CellPosition = cell;
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_PlayerStats = GetComponent<PlayerStats>();
        transform.position = m_Board.CellToWorld(cell);
    }

    void Update()
    {
        if (paused == false && GameManager.instance.gameStarted)
        {
            HandleInput();
            if (Mouse.current.leftButton.isPressed)
            {
                TryAttack();
            }
        }
    }

    void FixedUpdate()
    {
        if (!paused)
        {
            HandleMovement();
        }
        
    }

    private void HandleInput()
    {
        inputDirection = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) { inputDirection.y += 1f; animator.SetInteger("Direction", 4); }
        if (Keyboard.current.sKey.isPressed) { inputDirection.y -= 1f; animator.SetInteger("Direction", 1); }
        if (Keyboard.current.dKey.isPressed) { inputDirection.x += 1f; animator.SetInteger("Direction", 3); }
        if (Keyboard.current.aKey.isPressed) { inputDirection.x -= 1f; animator.SetInteger("Direction", 2); }
        if (!Keyboard.current.wKey.isPressed && !Keyboard.current.sKey.isPressed && !Keyboard.current.dKey.isPressed && !Keyboard.current.aKey.isPressed) animator.SetInteger("Direction", 0);


            inputDirection.Normalize();

        if (Keyboard.current.fKey.wasPressedThisFrame) m_PlayerStats.ConsumeItem(m_PlayerStats.consumable);
        if (Keyboard.current.escapeKey.wasPressedThisFrame) GameManager.instance.pause();
    }

    private void HandleMovement()
    {
        if (inputDirection == Vector2.zero) return;

        Vector2 newPosition = m_Rigidbody.position + inputDirection * m_PlayerStats.speed * Time.fixedDeltaTime;

        Vector2Int cellCoords = m_Board.WorldToCell(newPosition);
        
        m_Rigidbody.MovePosition(newPosition);

    }

    private void TryAttack()
    {
        float currentCooldown = attackCooldown * (1f - m_PlayerStats.cooldownRed);
        if (Time.time - lastAttackTime < currentCooldown) return;
        if (m_PlayerStats.currentMana < 5) return;

        lastAttackTime = Time.time;
        ShootProjectile();
    }

    private void ShootProjectile()
    {
        m_PlayerStats.currentMana -= 5;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0f;
        Vector2 direction = (mousePos - transform.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        float baseScale = 0.5f; 
        float scale = baseScale * (1f + m_PlayerStats.areaMult);
        proj.transform.localScale = Vector3.one * scale;

        proj.GetComponent<Projectile>().Init(
            direction,
            m_PlayerStats.damage,
            m_PlayerStats.projecRange,
            Projectile.ProjectileOwner.Player,
            5f * 1f - m_PlayerStats.projecSpeed
        );
    }
}
