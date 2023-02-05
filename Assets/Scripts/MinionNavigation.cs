using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionNavigation : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    [field: SerializeField] public LayerMask wallLayerMask;
    [field: SerializeField] public Transform Followtransform { get; set; }
    [field: SerializeField] public float MinDistance { get; set; } = 5f;
    [field: SerializeField] public string PlayerTag { get; set; } = "Player";
    [field: SerializeField] public string WaypointTag { get; set; } = "playerHordeWaypoint";
    [field: SerializeField] public string PlayerHordeAreaTag { get; set; } = "playerHordeSizeContainer";
    [field: SerializeField] public float BaseSpeed {get; set; } = 10f;
    [field: SerializeField] public float BaseAcceleration {get; set; } = 10f;
    private Transform WaypointTransform;
    private Transform PlayerTransform;
    private CapsuleCollider playerHorde;

    void Start()
    {
        var waypointGo = GameObject.FindGameObjectWithTag(WaypointTag);
        WaypointTransform = waypointGo.GetComponent<Transform>();
        var playerGo = GameObject.FindGameObjectWithTag(PlayerTag);
        PlayerTransform = playerGo.GetComponent<Transform>();
        Followtransform = WaypointTransform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        var playerHordeArea = GameObject.FindGameObjectWithTag(PlayerHordeAreaTag);
        playerHorde = playerHordeArea.GetComponent<CapsuleCollider>();
    }

    private bool isInHordeArea()
    {
        return playerHorde.bounds.Contains(transform.position);
    }

    private bool IsWaypointBlocked()
    {
        var direction = WaypointTransform.position - PlayerTransform.position;
        RaycastHit hit;
        return Physics.Raycast(
            PlayerTransform.position,
            direction,
            out hit,
            Vector3.Distance(WaypointTransform.position, PlayerTransform.position),
            wallLayerMask
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (Followtransform == null) return;

        Followtransform = WaypointTransform;

        if (IsWaypointBlocked())
        {
            Debug.Log("Waypoint blocked, using player");
            Followtransform = PlayerTransform;
        }

        var destination = Followtransform.position;
        
        _navMeshAgent.isStopped = Vector3.Distance(transform.position, destination) <= MinDistance;
        _navMeshAgent.destination = destination;
        _navMeshAgent.speed = (BaseSpeed / (1.25f / Vector3.Distance(transform.position, destination)));
        _navMeshAgent.acceleration = (BaseAcceleration / (1.25f / Vector3.Distance(transform.position, destination)));
    }
}
