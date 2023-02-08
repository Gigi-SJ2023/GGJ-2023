using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RangeTrigger: MonoBehaviour
{
    [SerializeField]
    private List<string> tags;

    public UnityEvent<GameObject> onTargetEnter;
    public UnityEvent<GameObject> onTargetExit;

    private void OnTriggerEnter(Collider other)
    {
        if (!tags.Any(other.CompareTag)) return;
        onTargetEnter?.Invoke(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!tags.Any(other.CompareTag)) return;
        onTargetExit?.Invoke(other.gameObject);
    }
}
