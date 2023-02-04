using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

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
        [field: SerializeField] protected SerializableHordeIntTypeDictionary StartingHordeMembersCount;
        [SerializeField] 
        private Spawn[] spawnables;

        protected static Dictionary<Spawn, ObjectPool<GameObject>> _goPools;
        private static readonly Vector3 DefaultGoSpawn = new Vector3(-100, -100, 0);
        protected Dictionary<HordeMemberType, Queue<GameObject>> _activeStacks;
        [SerializeField] private Vector2 maxSpawnDistance = Vector3.zero;
        [SerializeField] private UnityEvent OnAttack;

        public virtual void Start()
        {
            _goPools = new Dictionary<Spawn, ObjectPool<GameObject>>();
            _activeStacks = new Dictionary<HordeMemberType, Queue<GameObject>>();
            foreach(var member in StartingHordeMembersCount)
            {
                _activeStacks.Add(member.Key, new Queue<GameObject>());
                for (var i = 0; i < member.Value; i++)
                {
                    Spawn(member.Key);
                }
            }
        }

        public GameObject Spawn(HordeMemberType type)
        {
            var spawn = Array.Find(spawnables, (spawn) => spawn.type == type);
            if (spawn == null) return null;
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
            unit.SetActive(true);
            return unit;
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
            var randX = Random.Range(-maxSpawnDistance.x, maxSpawnDistance.x);
            var randZ = Random.Range(-maxSpawnDistance.y, maxSpawnDistance.y);
            var offset = new Vector3(randX, 0, randZ);
            go.transform.position = transform.position + offset;
        }
        
        private void OnReleaseGOToPool(GameObject go)
        {
            go.transform.position = DefaultGoSpawn;
            go.SetActive(false);
        }
    }
}