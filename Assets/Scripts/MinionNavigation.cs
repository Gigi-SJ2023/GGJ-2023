using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionNavigation : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    [field: SerializeField] public Transform Followtransform { get; set; }
    [field: SerializeField] public float MinDistance { get; set; } = 5f;
    [field: SerializeField] public string WaypointTag { get; set; } = "playerHordeWaypoint";
    [field: SerializeField] public string PlayerHordeAreaTag { get; set; } = "playerHordeSizeContainer";
    [field: SerializeField] public float BaseSpeed {get; set; } = 10f;
    [field: SerializeField] public float BaseAcceleration {get; set; } = 10f;
    private CapsuleCollider playerHorde;

    void Start()
    {
        var playerGo = GameObject.FindGameObjectWithTag(WaypointTag);
        Followtransform = playerGo.GetComponent<Transform>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        var playerHordeArea = GameObject.FindGameObjectWithTag(PlayerHordeAreaTag);
        playerHorde = playerHordeArea.GetComponent<CapsuleCollider>();
    }

    private bool isInHordeArea()
    {
        return playerHorde.bounds.Contains(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Followtransform == null) return;

        var destination = Followtransform.position;
        
        _navMeshAgent.isStopped = Vector3.Distance(transform.position, destination) <= MinDistance;
        _navMeshAgent.destination = destination;
        _navMeshAgent.speed = (BaseSpeed / (2 / Vector3.Distance(transform.position, destination)));
        _navMeshAgent.acceleration = (BaseAcceleration / (1.5f / Vector3.Distance(transform.position, destination)));
    }
}
