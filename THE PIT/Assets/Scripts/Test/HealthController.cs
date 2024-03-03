using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int startingHealth;

    private Health health;

    void Awake()
    {
        health = new Health(startingHealth);
    }

    private void Start()
    {
        UIEvents.Instance.ValueChanged(this, health.GetCurrentHealth());
    }

    public void OnHealthIncrease(int amount)
    {
        health.Increase(amount);
        UIEvents.Instance.ValueChanged(this, health.GetCurrentHealth());
    }

    public void OnHealthDecrease(int amount)
    {
        health.Decrease(amount);
        UIEvents.Instance.ValueChanged(this, health.GetCurrentHealth());
    }
}
