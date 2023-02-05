using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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
    private CapsuleCollider PlayerHordeCollider;
    private Transform PlayerHordeTransform;
    private PlayerController playerController;
    private bool playerIsMoving;

    void Start()
    {
        playerIsMoving = false;
        var waypointGo = GameObject.FindGameObjectWithTag(WaypointTag);
        WaypointTransform = waypointGo.GetComponent<Transform>();
        var playerGo = GameObject.FindGameObjectWithTag(PlayerTag);
        PlayerTransform = playerGo.GetComponent<Transform>();
        Followtransform = WaypointTransform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        var playerHordeArea = GameObject.FindGameObjectWithTag(PlayerHordeAreaTag);
        PlayerHordeCollider = playerHordeArea.GetComponent<CapsuleCollider>();
        PlayerHordeTransform = playerHordeArea.GetComponent<Transform>();
        playerController = playerGo.GetComponent<PlayerController>();
    }

    private bool IsInHordeArea()
    {
        return PlayerHordeCollider.bounds.Contains(transform.position);
    }

    private bool IsWaypointBlocked()
    // checks if there are walls between the player and the playerWaypoint
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

    private void PickRandomDestinationInHordeArea()
    // picks a random destination inside the player horde
    {
        var radius = PlayerHordeCollider.bounds.size.x / 2f;
        Vector3 randomPosition = Random.insideUnitCircle * radius;
        randomPosition += PlayerHordeTransform.position;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, radius, 1))
        {
            _navMeshAgent.destination = hit.position;
        }
    }

    public void DecideDestination()
    // decides whether to follow player or playerWaypoint
    {
        if (playerIsMoving)
        {
            Followtransform = WaypointTransform;

            if (IsWaypointBlocked())
            {
                Followtransform = PlayerTransform;
            }

            _navMeshAgent.destination = Followtransform.position;
        }
    }

    public void MoveTowardsDestination()
    {
        var distanceFromDestination = Vector3.Distance(transform.position, _navMeshAgent.destination);

        _navMeshAgent.isStopped = distanceFromDestination <= MinDistance;

        if (!playerIsMoving && _navMeshAgent.isStopped)
        {
            PickRandomDestinationInHordeArea();
        }

        _navMeshAgent.speed = (BaseSpeed / (1.25f / distanceFromDestination));
        _navMeshAgent.acceleration = (BaseAcceleration / (1.25f / distanceFromDestination));

        if (!playerIsMoving && IsInHordeArea())
        {
            _navMeshAgent.speed = BaseSpeed / 2;
            _navMeshAgent.acceleration = BaseAcceleration / 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerIsMoving = playerController.isMoving;
        DecideDestination();
        MoveTowardsDestination();
    }
}
