
using UnityEngine;

public abstract class AttackStats: ScriptableObject
{
    public abstract int GetDamage();
    public abstract float GetTickDuration();
}
