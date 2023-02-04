using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class HordeAttack: MonoBehaviour
    {
        private Timer _timer;

        [field: SerializeField] 
        private HordeMemberType HordeType { get; set; } = HordeMemberType.Carrot;
        [field: SerializeField] 
        public float TickDuration { get; set; } = 1;
        public int DamagePerUnit { get; set; } = 10;
        public int Unit { get; set; } = 1;
        [SerializeField]
        private bool active;
        [field: SerializeField] 
        private string enemyTag = "enemy";

        private List<Damageable> _targets = new List<Damageable>();
        
        private void Start()
        {
            _timer = new Timer
            {
                Duration = TickDuration,
                Paused = true
            };
            _timer.ONTimerEnd += ApplyDamage;
        }

        private void Update()
        {
            _timer.Tick(Time.deltaTime);
        }

        private void ApplyDamage()
        {
            if (!active) return;
            var damage = DamagePerUnit * Unit;
            _timer.Reset();

            Debug.Log(String.Format("Type {0} applying {1} damage.", HordeType, damage));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(enemyTag)) return;
            StartAttack();
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(enemyTag)) return;
            StopAttack();
        }

        private void StartAttack()
        {
            active = true;
            _timer.Reset();
            _timer.Paused = false;
        }

        private void StopAttack()
        {
            active = false;
            _timer.Reset();
            _timer.Paused = true;
        }
    }
}