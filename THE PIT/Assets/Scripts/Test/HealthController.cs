using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int startingHealth;
    
    [Header("Events")]
    public SpecificGameEvent onHealthChanged;

    private Health health;

    void Awake()
    {
        health = new Health(startingHealth);
    }

    private void Start()
    {
        onHealthChanged.Raise(this, health.GetCurrentHealth());
    }

    public void OnHealthIncrease(Component sender, object data)
    {
        if (data is int amount)
        {
            health.Increase(amount);
            onHealthChanged.Raise(this, health.GetCurrentHealth());
        }
    }

    public void OnHealthDecrease(Component sender, object data)
    {
        if (data is int amount)
        {
            health.Decrease(amount);
            onHealthChanged.Raise(this, health.GetCurrentHealth());
        }
    }
}
