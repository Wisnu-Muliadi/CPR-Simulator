using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerControl;

public class TutorialQuestTask : MonoBehaviour
{
    TutorialManager _tutorialManager;
    ICPRable cprable;
    IInteractable interactable;
    int _count = 0;
    int _repeatTarget;
    void Start()
    {
        _tutorialManager = TutorialManager.Instance;
    }
    public TutorialQuestTask ReadyTrigger(bool CPRable, int repeatCount)
    {
        switch (CPRable)
        {
            case true:
                if (TryGetComponent(out cprable)) cprable.InteractAction += TriggerTask;
                break;
            case false:
                if (TryGetComponent(out interactable)) interactable.InteractAction += TriggerTask;
                break;
        }
        _repeatTarget = repeatCount;
        return this;
    }
    public void TriggerTask()
    {
        if (++_count != _repeatTarget) return;
        _tutorialManager.QuestTaskSubtract(this);
    }
    void OnDestroy()
    {
        if(cprable != null) cprable.InteractAction -= TriggerTask;
        if (interactable != null) interactable.InteractAction -= TriggerTask;
        // housekeeping!
    }
}
