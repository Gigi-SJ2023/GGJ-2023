using System.Collections.Generic;
using UnityEngine;
using PlayerHorde;
using SerializableUtils;

namespace Loot
{
    class RecruitArea: MonoBehaviour
    {
        [field: SerializeField] public string PlayerHordeAreaTag { get; set; } = "playerHordeSizeContainer";
        [field: SerializeField] public string PlayerHordeTag { get; set; } = "playerHorde";
        private CapsuleCollider playerHordeCollider;
        private HordeController hordeController;
        [field: SerializeField]
        private HordeController recruitHordeController;
        private bool claimed = false;
        private SerializableHordeIntTypeDictionary RecruitCount;

        private void Start() {
            var playerHordeAreaGo = GameObject.FindGameObjectWithTag(PlayerHordeAreaTag);
            playerHordeCollider = playerHordeAreaGo.GetComponent<CapsuleCollider>();
            var playerHordeGo = GameObject.FindGameObjectWithTag(PlayerHordeTag);
            hordeController = playerHordeGo.GetComponent<HordeController>();
            recruitHordeController = gameObject.GetComponent<HordeController>();
            RecruitCount = recruitHordeController.hordeMembersCount;
        }

        private void Update()
        {
            if (!playerHordeCollider.bounds.Contains(transform.position) || claimed) return;
            claimed = true;
            var count = new Dictionary<HordeMemberType, int>(RecruitCount);
            foreach (var member in count)
            {
                for (var i = 0; i < member.Value; i++)
                {
                    hordeController.AddMemberToHorde(member.Key);
                    recruitHordeController.DestroyByType(member.Key);
                }
            }
            // Destroy(gameObject);
        }
    }
}