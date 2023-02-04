using System.Collections.Generic;
using PlayerHorde;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyHordeController: HordeController
{
    public override void Start()
    {
        var aggro = gameObject.GetComponent<EnemyAggro>();
        var enemyNavigations = new List<EnemyNavigation>();
        _goPools = new Dictionary<Spawn, ObjectPool<GameObject>>();
        _activeStacks = new Dictionary<HordeMemberType, Queue<GameObject>>();
        foreach(var member in StartingHordeMembersCount)
        {
            _activeStacks.Add(member.Key, new Queue<GameObject>());
            for (var i = 0; i < member.Value; i++)
            {
                var go =Spawn(member.Key);
                var navigation = go.GetComponent<EnemyNavigation>();
                enemyNavigations.Add(navigation);
            }
        }
        aggro.navigations = enemyNavigations;
    }
}
