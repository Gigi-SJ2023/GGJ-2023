using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerHorde
{
    public class HordeAttack: MonoBehaviour
    {
        private Timer _timer;
        [field: SerializeField] 
        private HordeMemberType HordeType { get; set; } = HordeMemberType.Carrot;
        [field: SerializeField] 
        public float TickDuration { get; set; } = 5;
        public int DamagePerUnit { get; set; } = 10;
        public int Unit { get; set; } = 1;
        [SerializeField]
        private bool active = false;
        [field: SerializeField] 
        private string enemyTag = "enemy";
        private float elapsed = 0;
        private Damageable _target;

        private void Update()
        {
            if (!active) return;
            elapsed += Time.deltaTime;
            if (!(elapsed > TickDuration)) return;
            elapsed = 0;
            ApplyDamage();
        }

        public int GetDamage()
        {
            return DamagePerUnit * Unit;
        }

        private void ApplyDamage()
        {
            if (!active) return;
            _target?.Damage(GetDamage());
            elapsed = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(enemyTag)) return;
            _target = other.gameObject.GetComponent<Damageable>();
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
            active = true;
            elapsed = 0;
        }

        private void StopAttack()
        {
            active = false;
            elapsed = 0;
        }
    }
}