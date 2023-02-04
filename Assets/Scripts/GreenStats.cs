using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayerHorde
{
    [CreateAssetMenu(fileName = "Spawn", menuName = "Greens/Stats")]
    public class GreenStats: ScriptableObject
    {
        public float BaseDamage;
        public float PercentageBonusPerUnit;
        public float TickDuration;
        public HordeMemberType GreenType;
        private int greensCount = 0;

        public void UpdateDamageStats(HordeMemberType type, int amount)
        {
            if(type == GreenType)
            {
                greensCount = amount;
            }
        }

        public int GetDamage()
        {
            return (int)Mathf.Ceil(BaseDamage + BaseDamage * PercentageBonusPerUnit * greensCount / 100);
        }
    }
}