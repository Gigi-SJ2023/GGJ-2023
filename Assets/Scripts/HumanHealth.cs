using System;
using UnityEngine;
using UnityEngine.Serialization;

public class HumanHealth: MonoBehaviour, Damageable
{
    public HumanStats stats;
    public GameObject humanGo;
    private int _currentHealth;
    private void Start()
    {
        _currentHealth = stats.Health;
    }

    public void Damage(int amount)
    {
        _currentHealth -= amount;
        if (_currentHealth > 0) return;
        _currentHealth = 0;
        Die();
    }
    private void Die()
    {
        humanGo.SetActive(false);
    }
}
