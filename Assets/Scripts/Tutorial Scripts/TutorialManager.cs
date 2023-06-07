using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    [System.Serializable]
    public class TutorialQuest
    {
        public UnityEvent Invokes;
        public bool OneForAll = false;
        public bool CPRable = false;
        public List<GameObject> QuestTasksObjects = new();
        [Tooltip("dont touch this one. for viewing only")]
        public List<TutorialQuestTask> QuestTasks = new();
    }
    [SerializeField]
    List<TutorialQuest> _tutorialQuests;
    [SerializeField, Tooltip("dont touch this either. for viewing only")] int _currentQuestIndex = 0;
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    void Start()
    {
        ExecuteQuest();
    }
    // Execute current quest
    private void ExecuteQuest()
    {
        if (_tutorialQuests.Count == 0) return;
        TutorialQuest quest = _tutorialQuests[_currentQuestIndex];
        foreach (GameObject obj in quest.QuestTasksObjects)
        {
            quest.QuestTasks.Add(obj.AddComponent<TutorialQuestTask>().ReadyTrigger(quest.CPRable));
        }
        quest.Invokes.Invoke();
    }
    // Immediately run next quest
    public void NextQuest()
    {
        if (_tutorialQuests[_currentQuestIndex].QuestTasks.TrueForAll(x => x != null))
        {
            foreach (TutorialQuestTask task in _tutorialQuests[_currentQuestIndex].QuestTasks)
                Destroy(task);
        }
        _currentQuestIndex++;
        ExecuteQuest();
    }
    public void QuestTaskSubtract(TutorialQuestTask questTask)
    {
        if (_tutorialQuests[_currentQuestIndex].OneForAll)
        {
            NextQuest(); return;
        }
        _tutorialQuests[_currentQuestIndex].QuestTasks.Remove(questTask);
        Destroy(questTask);
        if (_tutorialQuests[_currentQuestIndex].QuestTasks.TrueForAll(x => x == null)) NextQuest();
    }
}
