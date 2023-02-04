using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace PlayerHorde
{
    public enum HordeMemberType
    {
        Carrot,
        Potato,
        Radish,
        Onion,
        Human
    }
    public class HordeController : MonoBehaviour
    {
        [field: SerializeField] 
        public SerializableHordeIntTypeDictionary hordeMembersCount;
        [SerializeField] 
        private Spawn[] spawnables;

        [SerializeField] protected UnityEvent<HordeMemberType, int> onUpdateMember;

        protected static Dictionary<HordeMemberType, ObjectPool<GameObject>> _goPools;
        private static readonly Vector3 DefaultGoSpawn = new Vector3(-100, -100, 0);
        protected Dictionary<HordeMemberType, Queue<GameObject>> ActiveQueue;
        [SerializeField] private Vector2 maxSpawnDistance = Vector3.zero;
        public virtual void Start()
        {
            _goPools = new Dictionary<HordeMemberType, ObjectPool<GameObject>>();
            ActiveQueue = new Dictionary<HordeMemberType, Queue<GameObject>>();
            foreach(var member in hordeMembersCount)
            {
                onUpdateMember.Invoke(member.Key, hordeMembersCount[member.Key]);
                ActiveQueue.Add(member.Key, new Queue<GameObject>());
                for (var i = 0; i < member.Value; i++)
                {
                    Spawn(member.Key);
                }
            }
            
        }

        protected GameObject Spawn(HordeMemberType type)
        {
            var spawn = Array.Find(spawnables, (spawn) => spawn.type == type);
            if (spawn == null) return null;
            if (!_goPools.ContainsKey(type))
            {
                var pool = new ObjectPool<GameObject>(
                    spawn.SpawnUnit, 
                    OnTakeGOFromPool, 
                    OnReleaseGOToPool
                );
                _goPools.Add(type, pool);
            }
            var unit = _goPools[type].Get();
            ActiveQueue[type]?.Enqueue(unit);
            unit.SetActive(true);
            return unit;
        }
        public void DestroyByType(HordeMemberType type)
        {
            if (ActiveQueue[type].Count <=0) return;
            var go = ActiveQueue[type].Dequeue();
            if (go == null) return;
            hordeMembersCount[type]--;
            _goPools[type].Release(go);
            onUpdateMember.Invoke(type, hordeMembersCount[type]);
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