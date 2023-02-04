﻿
using PlayerHorde;
using UnityEngine;

public class HumanAttack : HordeAttack
{
    public HumanStats stats;

    public override float GetTickDuration() {
        return stats.TickDuration;
    }

    public override int GetDamage()
    {
        Debug.Log(stats.GetDamage());
        return stats.GetDamage();
    }
}
