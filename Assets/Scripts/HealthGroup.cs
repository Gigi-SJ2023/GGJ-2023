using System.Collections.Generic;
using System.Linq;
using PlayerHorde;
using Unity.VisualScripting;
using UnityEngine;

public class HealthGroup : MonoBehaviour, Damageable
{
    private HordeController _controller;
    private int _totalMembers = 0;
    private void Start()
    {
        _controller = gameObject.GetComponent<HordeController>();
    }

    public void Damage(int amount)
    {
        var members = _controller.hordeMembersCount;
        _totalMembers = members.Aggregate(0, (acc, item) => acc + item.Value);
        if (_totalMembers < 0) return;
        var target = FindTarget();
        _controller.DestroyByType(target);
    }

    private HordeMemberType FindTarget()
    {
        var rand = Random.Range(0, _totalMembers);
        var members = _controller.hordeMembersCount;
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
