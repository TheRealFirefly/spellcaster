using System;
using UnityEngine;
using UnityEngine.Rendering;

public class TickManager : MonoBehaviour
{
    public PlayerStats Player;

    public float tickInterval = 1f;
    private float tickTimer = 0f;

    public static TickManager instance;

    public event Action OnTick;
    void Awake()
    {
        if (instance != null && instance != this)
        { 
            Destroy(gameObject); 
            return; 
        }
        instance = this; 
    }

    void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= tickInterval)
        { 
            tickTimer -= tickInterval;
            OnTick?.Invoke();
        }
    }
}
