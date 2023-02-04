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
        private bool claimed = false;

        [field: SerializeField] 
        public SerializableHordeIntTypeDictionary RecruitCount;

        private void Start() {
            var playerHordeAreaGo = GameObject.FindGameObjectWithTag(PlayerHordeAreaTag);
            playerHordeCollider = playerHordeAreaGo.GetComponent<CapsuleCollider>();
            var playerHordeGo = GameObject.FindGameObjectWithTag(PlayerHordeTag);
            hordeController = playerHordeGo.GetComponent<HordeController>();
        }

        private void Update() {
            if (playerHordeCollider.bounds.Contains(transform.position) && !claimed)
            {
                claimed = true;
                foreach (var member in RecruitCount)
                {
                    for (var i = 0; i < member.Value; i++)
                    {
                        hordeController.AddMemberToHorde(member.Key);
                    }
                }
                Destroy(gameObject);
            }
        }
    }
}