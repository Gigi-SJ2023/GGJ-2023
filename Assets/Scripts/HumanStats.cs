using System;
using System.Collections.Generic;
using System.Linq;
using PlayerHorde;
using UnityEngine;
public enum HumanType
{
    Antonio,
    Clara,
    Roberto
}
[CreateAssetMenu(fileName = "Spawn", menuName = "Humans/Stats")]
public class HumanStats : AttackStats
{
    public float TickDuration;
    public int Health;
    public int AttackBonus;
    public int Speed;
    public int Range;
    [Range(0.0f, 1f)]
    public float sizePercentage = 0.24f;
    private Dictionary<HordeMemberType, int> hordeMembersCount = new Dictionary<HordeMemberType, int>();

    public void UpdateDamageStats(HordeMemberType type, int amount)
    {
        if(!hordeMembersCount.TryAdd(type, amount))
        {
            hordeMembersCount[type] = amount;
        }
    }

    private int HordeSize()
    {
        return hordeMembersCount.Aggregate(0, (acc, item) => acc + item.Value);
    }
    
    public override int GetDamage()
    {
        return (int)Math.Floor(1 + HordeSize() * sizePercentage) + AttackBonus;
    }

    public override float GetTickDuration()
    {
        return TickDuration;
    }
}
