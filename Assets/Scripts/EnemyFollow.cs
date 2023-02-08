using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    Idling,
    Seeking,
    Attacking
}

public class EnemyFollow : MonoBehaviour
{
    [field: SerializeField] public LayerMask wallLayerMask;
    [field: SerializeField] public EnemyNavigation navigation;
    private EnemyState _currentState = EnemyState.Idling;
    private Transform FollowTransform { get; set; }
    private void Update()
    {
        if (_currentState == EnemyState.Idling) return;
        SeekForPlayer();
    }
    // Start is called before the first frame update
    private void SeekForPlayer()
    {
        if (navigation == null) return;
        var direction = FollowTransform.position - transform.position;
        if (IsBeyondWall(direction)) return;
        navigation.currentState = EnemyState.Attacking;
        _currentState = EnemyState.Attacking;
    }

    private bool IsBeyondWall(Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, Mathf.Infinity, wallLayerMask);
    }

    private void OnAggroEnter(GameObject go)
    {
        FollowTransform = go.GetComponent<Transform>();
        if (FollowTransform == null) return;
        _currentState = EnemyState.Seeking;
    }
    private void OnAggroExit(GameObject go)
    {
        if (_currentState != EnemyState.Seeking) return;
        _currentState = EnemyState.Idling;
    }
}
