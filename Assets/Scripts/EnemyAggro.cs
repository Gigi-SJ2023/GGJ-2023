using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    Idling,
    Seeking,
    Attacking
}
public class EnemyAggro : MonoBehaviour
{
    [field: SerializeField] private EnemyState currentState = EnemyState.Seeking;
    [field: SerializeField] public LayerMask wallLayerMask;
    [field: SerializeField] public string PlayerTag { get; set; } = "Player";
    [field: SerializeField] public Transform Followtransform { get; set; }
    [field: SerializeField] public List<EnemyNavigation> navigations;

    private void Awake()
    {
        var playerGo = GameObject.FindGameObjectWithTag(PlayerTag);
        Followtransform = playerGo.GetComponent<Transform>();
    }
    void Update()
    {
        if (currentState != EnemyState.Seeking) return;
        SeekForPlayer();
        if (currentState != EnemyState.Attacking) return;
    }
    // Start is called before the first frame update
    void SeekForPlayer()
    {
        var direction = Followtransform.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, wallLayerMask))
        {
            Debug.Log("Seeking for player: not found");
            return;
        }
        // setup children for attack
        Debug.Log("Seeking for player: found");
        foreach (var navigation in navigations)
        {
            navigation.currentState = EnemyState.Attacking;
        }
        currentState = EnemyState.Attacking;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PlayerTag)) return;
        Debug.Log("Seeking for player: started");
        currentState = EnemyState.Seeking;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(PlayerTag)) return;
        if (currentState != EnemyState.Seeking) return;
        Debug.Log("Going back to idle");
        currentState = EnemyState.Idling;
    }
}
