using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionNavigation : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    [field: SerializeField] public Transform Playertransform { get; set; }
    [field: SerializeField] public float MinDistance { get; set; } = 5f;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        var destination = Playertransform.position;
        _navMeshAgent.isStopped = Vector3.Distance(transform.position, destination) <= MinDistance;
        _navMeshAgent.destination = destination;
    }
}
