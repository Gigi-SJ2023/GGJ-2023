using UnityEngine;

namespace PlayerHorde
{
    public class HordeAttack: MonoBehaviour
    {
        [field: SerializeField] 
        private HordeMemberType HordeType { get; set; } = HordeMemberType.Carrot;
        [field: SerializeField] 
        public float TickDuration { get; set; } = 5;
        public int DamagePerUnit { get; set; } = 10;
        public int Unit { get; set; } = 1;
        [field: SerializeField] 
        private string enemyTag = "enemy";
        [SerializeField]
        private LayerMask wallLayerMask;
        private float elapsed = 0;
        private Damageable _target;
        private Transform _targetTransform;
        private EnemyState state = EnemyState.Idling;

        private void Update()
        {
            if (state is EnemyState.Seeking or EnemyState.Attacking) SeekForTarget();
            if (state != EnemyState.Attacking) return;
            elapsed += Time.deltaTime;
            if (!(elapsed > TickDuration)) return;
            elapsed = 0;
            ApplyDamage();
        }

        public void UpdateAttackStats(HordeMemberType memberType, int amount)
        {
            if (memberType != HordeType) return;
            Unit = amount;
        }

        private void SeekForTarget()
        {
            var direction = _targetTransform.position - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, wallLayerMask))
            {
                state = EnemyState.Seeking;
                return;
            }
            state = EnemyState.Attacking;
        }

        public int GetDamage()
        {
            return DamagePerUnit * Unit;
        }

        private void ApplyDamage()
        {
            _target?.Damage(GetDamage());
            elapsed = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(enemyTag)) return;
            _target = other.gameObject.GetComponent<Damageable>();
            _targetTransform = other.gameObject.GetComponent<Transform>();
            if (_target == null) return;
            StartAttack();
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(enemyTag)) return;
            _target = null;
            StopAttack();
        }

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