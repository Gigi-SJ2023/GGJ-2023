using UnityEngine;

namespace PlayerHorde
{
    public class GreenAttack : HordeAttack
    {
        public GreenStats stats;

        public override float GetTickDuration() {
            return stats.TickDuration;
        }

        public override int GetDamage()
        {
            Debug.Log(stats.GetDamage());
            return stats.GetDamage();
        }
    }
}