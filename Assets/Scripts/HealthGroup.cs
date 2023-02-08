using System.Collections.Generic;
using System.Linq;
using PlayerHorde;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class HealthGroup : MonoBehaviour, IDamageable
{
    private int _totalMembers = 0;
    private Dictionary<HordeMemberType, int> _hordeMembersCount;
    public UnityEvent<HordeMemberType> onDestroyByType;
    [field: SerializeField]
    public UnityEvent<IDamageable> OnDeath { get; set; }

    private void Start()
    {
        _hordeMembersCount = new Dictionary<HordeMemberType, int>();
    }

    public void UpdateStats(HordeMemberType type, int amount)
    {
        if(!_hordeMembersCount.TryAdd(type, amount))
        {
            _hordeMembersCount[type] = amount;
        }
    }

    public void Damage(int amount)
    {
        _totalMembers = _hordeMembersCount.Aggregate(0, (acc, item) => acc + item.Value);
        if (_totalMembers - amount < 0) OnDeath?.Invoke(this);
        if (_totalMembers < 0) return;
        var target = FindTarget();
        onDestroyByType?.Invoke(target);
    }

    private HordeMemberType FindTarget()
    {
        var rand = Random.Range(0, _totalMembers);
        var members = _hordeMembersCount;
        var intervals =  new Dictionary<HordeMemberType, int>();
        var acc = 0;
        foreach (var member in members.Where(member => member.Value > 0))
        {
            acc += member.Value;
            intervals.Add(member.Key, acc);
        }
        var target = intervals.FirstOrDefault((item) => item.Value >= rand);
        return target.Key;
    }
}
