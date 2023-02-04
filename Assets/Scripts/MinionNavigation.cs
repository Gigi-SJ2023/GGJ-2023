using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionNavigation : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    [field: SerializeField] public Transform Followtransform { get; set; }
    [field: SerializeField] public float MinDistance { get; set; } = 5f;
    [field: SerializeField] public string PlayerTag { get; set; } = "player";

    void Start()
    {
        var playerGo = GameObject.FindGameObjectWithTag(PlayerTag);
        Debug.Log(playerGo);
        Followtransform = playerGo.GetComponent<Transform>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Followtransform == null) return;
        var destination = Followtransform.position;
        _navMeshAgent.isStopped = Vector3.Distance(transform.position, destination) <= MinDistance;
        _navMeshAgent.destination = destination;
    }
}
