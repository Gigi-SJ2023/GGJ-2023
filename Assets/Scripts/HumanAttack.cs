
using PlayerHorde;
using UnityEngine;

public class HumanAttack : HordeAttack
{
    public HumanStats stats;

    public override int GetDamage()
    {
        Debug.Log(stats.GetDamage());
        return stats.GetDamage();
    }
}
