using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerHorde
{
    public enum HordeMemberType
    {
        Carrot,
        Potato,
        Radish,
        Onion
    }

    public class HordeController : MonoBehaviour
    {
        [field: SerializeField] 
        private SerializableHordeIntTypeDictionary StartingHordeMembersCount;
        private IDictionary<HordeMemberType, int> hordeMembersCount;
        private IDictionary<HordeMemberType, int> previousHordeMembersCount;
        private IDictionary<HordeMemberType, Stack<GameObject>> instantiatedObjects;
        public GameObject player;
        public GameObject minionPrefab;

        private void Start() {
            hordeMembersCount = new Dictionary<HordeMemberType, int>();
            foreach (KeyValuePair<HordeMemberType, int> kvp in StartingHordeMembersCount)
            {
                hordeMembersCount[kvp.Key] = kvp.Value;
            }
            previousHordeMembersCount = new Dictionary<HordeMemberType, int>();
            instantiatedObjects = new Dictionary<HordeMemberType, Stack<GameObject>>();
            
            foreach (HordeMemberType type in (HordeMemberType[]) Enum.GetValues(typeof(HordeMemberType)))
            {
                previousHordeMembersCount.Add(type, 0);
            }
            OnMemberCountChange();
        }

        private int CalcTotalMemberCount()
        {
            int total = 0;
            foreach (int subTotal in hordeMembersCount.Values)
            {
                total += subTotal;
            }
            return total;
        }

        public void AddHordeMember(HordeMemberType type, int qty = 1)
        {
            if (!hordeMembersCount.ContainsKey(type))
            {
                hordeMembersCount[type] = 0;
            }
            hordeMembersCount[type]++;
            OnMemberCountChange();
        }

        private void InstantiateNewHordeMember(HordeMemberType Type)
        {
            Debug.Log("InstantiateNewHordeMember");
            GameObject newMember = Instantiate<GameObject>(minionPrefab, player.transform) as GameObject;

            MinionNavigation minionNavigation = newMember.getComponent<MinionNavigation>();

            minionNavigation.Playertransform = player.transform;
            instantiatedObjects[Type].Push(newMember);
            hordeMembersCount[Type]++;
        }

        private void DestroyHordeMember(HordeMemberType Type)
        {
            Debug.Log("DestroyHordeMember");
            if (instantiatedObjects[Type].Count > 0)
            {
                Destroy(instantiatedObjects[Type].Pop());
                hordeMembersCount[Type]--;
            }
        }

        public void OnMemberCountChange()
        {
            Debug.Log("OnMemberCountChange");
            foreach (KeyValuePair<HordeMemberType, int> kvp in hordeMembersCount)
            {
                while (previousHordeMembersCount[kvp.Key] != kvp.Value)
                {
                    if (previousHordeMembersCount[kvp.Key] < kvp.Value)
                    {
                        InstantiateNewHordeMember(kvp.Key);
                    }
                    else
                    {
                        DestroyHordeMember(kvp.Key);
                    }
                }
            }
        }
    }
}