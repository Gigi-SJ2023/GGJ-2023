using UnityEngine.Events;

public interface IDamageable
{
    public UnityEvent<IDamageable> OnDeath { get; set; }
    public void Damage(int amount);
}
