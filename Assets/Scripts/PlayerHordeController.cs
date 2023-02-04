using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerHorde
{
    class PlayerHordeController: MonoBehaviour
    {
        public float minRange = 1f;
        public float maxRange = 10.5f;
        public float minBonusRange = 0f;
        public float maxBonusRange = 1.5f;
        public float minAggro = 0f;
        public float maxAggro = 100f;
        public float minSpeed = 8f;
        public float maxSpeed = 20f;
        
        private Dictionary<HordeMemberType, int> hordeMembersCount;

        private void Start() {
            hordeMembersCount = new Dictionary<HordeMemberType, int>();
        }

        public int calcHordeSize()
        {
            return hordeMembersCount.Aggregate(0, (acc, item) => acc + item.Value);
        }

        private float calcBaseHordeRange()
        {
            var value = 0.5f + (calcHordeSize() * 0.1f);
            return Mathf.Min(minRange, Mathf.Max(value, maxRange));
        }

        private float calcBonusRange()
        {
            var value = 0.3f + (calcHordeSize() / 20);
            return Mathf.Min(minRange, Mathf.Max(value, maxRange));
        }

        public float calcTotalHordeRange()
        {
            return calcBaseHordeRange() + calcBonusRange();
        }

        public float calcAggro()
        {
            return 1 - (1 / calcHordeSize());
        }

        public void onUpdateHordeMembers(HordeMemberType type, int amount)
        {
            if(!hordeMembersCount.TryAdd(type, amount))
            {
                hordeMembersCount[type] = amount;
            }
        }
    }
}