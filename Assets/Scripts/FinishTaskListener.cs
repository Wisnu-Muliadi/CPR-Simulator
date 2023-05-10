using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishTaskListener : MonoBehaviour
{
    public UnityEvent<int> EventFinishTask { get; private set; } = new();
    public int taskIndex;

    public void FinishTask()
    {
        EventFinishTask.Invoke(taskIndex);
    }
}
