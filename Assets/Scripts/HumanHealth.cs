using UnityEngine;
using UnityEngine.Events;


public class HumanHealth: MonoBehaviour, IDamageable
{
    public HumanStats stats;
    public GameObject humanGo;
    private int _currentHealth;
    [field: SerializeField]
    public UnityEvent<IDamageable> OnDeath { get; set; }
    
    private void Start()
    {
        _currentHealth = stats.Health;
    }
    
    public void Damage(int amount)
    {
        _currentHealth -= amount;
        if (_currentHealth > 0) return;
        _currentHealth = 0;
        OnDeath?.Invoke(this);
    }
}
