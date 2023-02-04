
using PlayerHorde;
using UnityEngine;

public class HumanAttack : HordeAttack
{
    public HumanStats stats;

    public override int GetDamage()
    {
        return stats.GetDamage();
    }
}
