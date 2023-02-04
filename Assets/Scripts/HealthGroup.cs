using System;
using System.Collections.Generic;
using System.Linq;
using PlayerHorde;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HealthGroup : MonoBehaviour, Damageable
{
    private int _totalMembers = 0;
    private Dictionary<HordeMemberType, int> hordeMembersCount;
    public UnityEvent<HordeMemberType> onDestroyByType;
    private void Start()
    {
        hordeMembersCount = new Dictionary<HordeMemberType, int>();
    }

    public void UpdateStats(HordeMemberType type, int amount)
    {
        if(!hordeMembersCount.TryAdd(type, amount))
        {
            hordeMembersCount[type] = amount;
        }
    }

    public void Damage(int amount)
    {
        _totalMembers = hordeMembersCount.Aggregate(0, (acc, item) => acc + item.Value);
        if (_totalMembers < 0) return;
        var target = FindTarget();
        onDestroyByType?.Invoke(target);
    }

    private HordeMemberType FindTarget()
    {
        var rand = Random.Range(0, _totalMembers);
        var members = hordeMembersCount;
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
