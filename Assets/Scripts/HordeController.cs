using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

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
        [SerializeField] 
        private Spawn[] spawnables;
        private static Dictionary<Spawn, ObjectPool<GameObject>> _goPools;
        private static readonly Vector3 DefaultGoSpawn = new Vector3(-100, -100, 0);
        public GameObject playerGO;
        private Dictionary<HordeMemberType, Queue<GameObject>> _activeStacks;
        private void Start()
        {
            _goPools = new Dictionary<Spawn, ObjectPool<GameObject>>();
            foreach(var member in StartingHordeMembersCount)
            {
                for (var i = 0; i < member.Value; i++)
                {
                    Spawn(member.Key);
                }
            }
        }

        public void Spawn(HordeMemberType type)
        {
            var spawn = Array.Find(spawnables, (spawn) => spawn.type == type);
            if (spawn == null) return;
            if (!_goPools.ContainsKey(spawn))
            {
                var pool = new ObjectPool<GameObject>(
                    spawn.SpawnUnit, 
                    OnTakeGOFromPool, 
                    OnReleaseGOToPool
                );
                _goPools.Add(spawn, pool);
            }
            var unit = _goPools[spawn].Get();
            var actives = _activeStacks[type];
            actives.Enqueue(unit);
            var navigation = unit.GetComponent<MinionNavigation>();
            navigation.Playertransform = playerGO.transform;
            unit.SetActive(true);
        }

        public void DestroyByType(HordeMemberType type)
        {
            var go = _activeStacks[type].Dequeue();
            var spawn = Array.Find(spawnables, (spawn) => spawn.type == type);
            if (spawn == null || go == null) return;
            _goPools[spawn].Release(go);
        }

        private void OnTakeGOFromPool(GameObject go)
        {
            go.transform.position = transform.position;
        }
        
        private static void OnReleaseGOToPool(GameObject go)
        {
            go.transform.position = DefaultGoSpawn;
            go.SetActive(false);
        }
    }
}