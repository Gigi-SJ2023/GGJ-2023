using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    [field: SerializeField] public Transform Followtransform { get; set; }
    [field: SerializeField] public float MinDistance { get; set; } = 5f;
    [field: SerializeField] public string PlayerTag { get; set; } = "Player";
    [field: SerializeField] public EnemyState currentState = EnemyState.Seeking;

    private void Awake()
    {
        var playerGo = GameObject.FindGameObjectWithTag(PlayerTag);
        Followtransform = playerGo.GetComponent<Transform>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentState != EnemyState.Attacking) return;
        _navMeshAgent.isStopped = false;
        _navMeshAgent.destination = Followtransform.position;
    }
}
