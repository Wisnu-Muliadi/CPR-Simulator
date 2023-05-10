using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UserInterface;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private TaskUILogic _taskUI;
    [SerializeField] private List<bool> _completionBools = new();
    [SerializeField] private UnityEvent AllTasksCompleted;

    #region Serializable Struct
    [System.Serializable]
    struct MainTask
    {
        [System.Serializable]
        public struct IndividualTask
        {
            [Tooltip("Objects need to Get [FinishTaskListener] and Call FinishTask()")]
            public GameObject[] listenerObjects;
            public int taskIndex;
        }
        [Tooltip("Max Amount is the [List Size] of [TaskUILogic]")]
        public IndividualTask[] tasks;
        public UnityEvent NewTaskEvent;
    }
    #endregion
    [Tooltip("Minimum amount is the [Tasks] List Count of [TaskUILogic]")]
    [SerializeField] private MainTask[] _finishTaskListeners;

    [Space, Header("for Testing in Editor")]
    [SerializeField] bool _refresh;
    [SerializeField] int _setTrueIndex;
    void Start()
    {
        for (int i = 0; i < _taskUI.ListSize; i++)
        {
            _completionBools.Add(false);
        }
        ResetBools();
        UpdateTaskListeners();
    }
    // testing
    void Update()
    {
        if (_refresh)
        {
            FinishTaskBool(_setTrueIndex);
            _refresh = false;
        }
    }
    public void FinishTaskBool(int index)
    {
        if (index >= _completionBools.Count) return;
        _completionBools[index] = true;

        if (_completionBools.TrueForAll(x => x))
        {
            AdvanceTask();
        }
        else
        {
            _taskUI.TaskFade(index);
        }
    }
    private void AdvanceTask()
    {
        _taskUI.CurrentTaskIndex++;
        if (_taskUI.CurrentTaskIndex < _taskUI.tasks.Count)
        {
            _taskUI.RefreshTask();
            ResetBools();
            UpdateTaskListeners();
        }
        else
        {
            AllTasksCompleted.Invoke();
        }
    }
    private void ResetBools()
    {
        for (int i = 0; i < _completionBools.Count; i++)
        {
            _completionBools[i] = i >= _taskUI.tasks[_taskUI.CurrentTaskIndex].Tasks.Length;
        }
    }
    private void UpdateTaskListeners()
    {
        // No more Listeners to Deploy
        if (_finishTaskListeners.Length <= _taskUI.CurrentTaskIndex) return;
        _finishTaskListeners[_taskUI.CurrentTaskIndex].NewTaskEvent.Invoke();
        foreach (MainTask.IndividualTask tasks in _finishTaskListeners[_taskUI.CurrentTaskIndex].tasks)
        {
            foreach (GameObject listenerObject in tasks.listenerObjects)
            {
                listenerObject.AddComponent<FinishTaskListener>().taskIndex = tasks.taskIndex;
                listenerObject.GetComponent<FinishTaskListener>().EventFinishTask.AddListener(UpdateTaskListeners);
            }
        }
    }
    // buggy need rework
    public void UpdateTaskListeners(int index)
    {
        if (_taskUI.CurrentTaskIndex >= _finishTaskListeners.Length) return;
        foreach (GameObject listenerObject in _finishTaskListeners[_taskUI.CurrentTaskIndex].tasks[index].listenerObjects)
            Destroy(listenerObject.GetComponent<FinishTaskListener>());
        FinishTaskBool(index);
    }
}
