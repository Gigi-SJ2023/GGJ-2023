using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attack: MonoBehaviour
{
    [field: SerializeField] 
    private AttackStats attackStats;
    [field: SerializeField] public LayerMask wallLayerMask;
    private Dictionary<int, (IDamageable, Transform)> _targets;
    private Dictionary<int, Transform> _transforms;
    private List<int> _deadTargets;
    private Timer _timer;
    private void Awake()
    {
        _targets = new Dictionary<int, (IDamageable, Transform)>();
        _timer = new Timer(attackStats.GetTickDuration());
        _deadTargets = new List<int>();
    }

    private void OnEnable()
    {
        _timer.ONTimerEnd += RemoveDeadTargets;
        _timer.ONTimerEnd += ApplyDamage;
        _timer.ONTimerEnd += _timer.Reset;
    }

    private void Update()
    {
        _timer.Tick(Time.deltaTime);
    }
    
    private bool IsBeyondWall(Vector3 direction, float distance)
    {
        return Physics.Raycast(transform.position, direction, distance, wallLayerMask);
    }


    private void ApplyDamage()
    {
        foreach (var (target, _transform) in _targets.Values)
        {
            var direction = transform.position - _transform.position;
            var distance = Vector3.Distance(transform.position, _transform.position);
            if (IsBeyondWall(direction, distance)) return;
            target.Damage(attackStats.GetDamage());
        }
    }

    private void RemoveDeadTargets()
    {
        foreach (var dead in _deadTargets)
        {
            _targets.Remove(dead);
        }
    }

    public void OnAttackRangeEnter(GameObject go)
    {
        var damageable = go.GetComponent<IDamageable>();
        var _transform = go.GetComponent<Transform>();
        if (damageable == null) return;
        _targets.Add(damageable.GetHashCode(), (damageable, _transform));
        damageable.OnDeath?.AddListener(OnTargetDeath);
        _timer.Paused = false;
    }
    
    public void OnAttackRangeExit(GameObject go)
    {
        var damageable = go.GetComponent<IDamageable>();
        if (damageable == null) return;
        _targets.Remove(damageable.GetHashCode());
        if (_targets.Any()) return;
        _timer.Reset();
        _timer.Paused = true;
    }

    private void OnDisable()
    {
        _timer.ONTimerEnd -= RemoveDeadTargets;
        _timer.ONTimerEnd -= ApplyDamage;
        _timer.ONTimerEnd -= _timer.Reset;
        foreach (var (target, _)  in _targets.Values)
        {
            target.OnDeath?.RemoveListener(OnTargetDeath);
        }
    }

    private void OnTargetDeath(IDamageable damageable)
    {
        _deadTargets.Add(damageable.GetHashCode());
        damageable.OnDeath?.RemoveListener(OnTargetDeath);
    }
}