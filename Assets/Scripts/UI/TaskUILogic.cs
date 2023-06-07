using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UserInterface
{
    public class TaskUILogic : MonoBehaviour
    {
        [SerializeField] GameObject _taskInstance;
        [SerializeField] Animation _currentAnimation; // still hardcoded animation player. could be better
        [SerializeField] bool _immediatelySlideIn;
        public int ListSize = 3;
        public int CurrentTaskIndex = 0;

        private readonly List<GameObject> _taskListInstances = new();
        private readonly List<Animation> _tasksAnimations = new();// still hardcoded animation player. could be better
        public List<TaskScriptableObject> tasks = new();
        
        [Space, Header("for Testing in Editor")]
        [SerializeField] private bool _refreshing = false;

        void Start()
        {
            for (int i = 0; i < ListSize; i++)
            {
                _taskListInstances.Add(Instantiate(_taskInstance, _taskInstance.transform.parent.transform));
                _tasksAnimations.Add(_taskListInstances[i].GetComponent<Animation>());
            }
            _taskInstance.SetActive(false);
            _currentAnimation.Play("Task UI Hiding");
            if (_immediatelySlideIn) EnterTaskUI();
        }
        void Update()
        {
            // for testing in Editor
            if (_refreshing)
            {
                RefreshTask();
                _refreshing = false;
            }
            enabled = false;
        }
        public void RefreshTask()
        {
            // If There's no more Task after this, get out
            if (CurrentTaskIndex >= tasks.Count) return;
            // Enabling and setting each lists and texts according to tasks
            for (int i = 0; i < ListSize; i++)
            {
                if (i < tasks[CurrentTaskIndex].Tasks.Length)
                {
                    _taskListInstances[i].SetActive(true);
                    _taskListInstances[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = tasks[CurrentTaskIndex].Tasks[i];
                    _tasksAnimations[i].Play("Task UI Pulse");
                }
                else
                {
                    _taskListInstances[i].SetActive(false);
                }
            }
        }
        public void TaskFade(int index)
        {
            _tasksAnimations[index].Play("Task UI Fade");
        }
        public void TaskPulseIn(int index)
        {
            _tasksAnimations[index].Play("Task UI Pulse");
        }
        public void TaskBarPulse()
        {
            _currentAnimation.Play("Task UI Pulse");
        }

        // presumably unused
        public bool IsInstanceActive(int index)
        {
            return _taskListInstances[index].activeSelf;
        }
        public void EnterTaskUI()
        {
            _currentAnimation.Play("Task UI Slide In");
        }
    }
}