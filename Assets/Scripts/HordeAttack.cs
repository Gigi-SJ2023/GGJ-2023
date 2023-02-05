using System;
using UnityEngine;

namespace PlayerHorde
{
    public class HordeAttack: MonoBehaviour
    {
        [field: SerializeField] 
        private HordeMemberType HordeType { get; set; } = HordeMemberType.Carrot;
        public int DamagePerUnit { get; set; } = 0;
        public int Unit { get; set; } = 1;
        [field: SerializeField] 
        private string enemyTag = "enemyHittable";
        [SerializeField]
        private LayerMask wallLayerMask;
        private float elapsed = 0;
        private Damageable _target;
        // private Transform _targetTransform;
        private EnemyState state = EnemyState.Idling;
        public bool debug = false;
        public GameObject[] hittables;
        public float range = 10f;
        public void Awake()
        {
            hittables = GameObject.FindGameObjectsWithTag(enemyTag);
            if (gameObject.name == "CarrotAttack")
            {
                Debug.Log(hittables);
            }
        }

        private void Update()
        {
            SeekForTarget();
            // if (state is EnemyState.Seeking or EnemyState.Attacking) 
            if (state != EnemyState.Attacking) return;
            elapsed += Time.deltaTime;
            if (!(elapsed > GetTickDuration())) return;
            elapsed = 0;
            
            ApplyDamage();
        }

        public virtual float GetTickDuration()
        {
            return 0;
        }

        public void UpdateAttackStats(HordeMemberType memberType, int amount)
        {
            if (memberType != HordeType) return;
            Unit = amount;
        }

        private void SeekForTarget()
        {
            foreach (var hittable in hittables)
            {
                var _targetTransform = hittable.transform;
                var direction = _targetTransform.position - transform.position;
                var distance = Vector3.Distance(_targetTransform.position, transform.position);
                RaycastHit hit;
                if ((distance > range)) continue;
                if (Physics.Raycast(transform.position, direction, out hit, range, wallLayerMask)) continue;
                state = EnemyState.Attacking;
                _target = hittable.GetComponent<Damageable>();
            }
        }

        public virtual int GetDamage()
        {
            return DamagePerUnit * Unit;
        }

        private void ApplyDamage()
        {
            
            var damage = GetDamage();
            if (debug) Debug.Log(String.Format("{0} attacked for {1} damage", gameObject.name, damage));
            _target?.Damage(GetDamage());
            elapsed = 0;
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (!other.CompareTag(enemyTag)) return;
        //     _target = other.gameObject.GetComponent<Damageable>();
        //     _targetTransform = other.gameObject.GetComponent<Transform>();
        //     if (_target == null) return;
        //     StartAttack();
        // }
        // private void OnTriggerExit(Collider other)
        // {
        //     if (!other.CompareTag(enemyTag)) return;
        //     _target = null;
        //     StopAttack();
        // }

        private void StartAttack()
        {
            state = EnemyState.Attacking;
            elapsed = 0;
        }

        private void StopAttack()
        {
            state = EnemyState.Idling;
            elapsed = 0;
        }
    }
}