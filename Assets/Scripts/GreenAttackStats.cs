using PlayerHorde;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn", menuName = "Stats/Greens")]
public class GreenAttackStats: AttackStats
{
    public float BaseDamage;
    public float PercentageBonusPerUnit;
    private float TickDuration { get; set; }
    public HordeMemberType GreenType;
    private int greensCount = 0;

    public void UpdateDamageStats(HordeMemberType type, int amount)
    {
        if(type == GreenType) greensCount = amount;
    }

    public override int GetDamage()
    {
        if (greensCount <= 0) return 0;
        return (int)Mathf.Ceil(BaseDamage + BaseDamage * PercentageBonusPerUnit * greensCount / 100);
    }

    public override float GetTickDuration()
    {
        return TickDuration;
    }
}