using System;
using UnityEngine;

[Serializable]
public class Timer
{
    [SerializeField]
    public float Duration { get; set; } = 0;
    public float ElapsedTime {get; private set;} = 0;
    public Action ONTimerEnd;
    public bool Paused { get; set; } = false;

    ~Timer()
    {
        Clear();
    }

    public void Reset() {
        ElapsedTime = 0;
    }

    public void Clear()
    {
        Reset();
        var delegates = ONTimerEnd.GetInvocationList();
        foreach (var d in delegates)
        {
            ONTimerEnd -= (d as Action);
        }
    }

    public float Tick(float deltaTime = 0f)
    {
        if (Paused) {
            return ElapsedTime;
        }

        ElapsedTime += deltaTime;

        if (ElapsedTime >= Duration) {
            ONTimerEnd?.Invoke();
        } 
        return ElapsedTime;
    }
}