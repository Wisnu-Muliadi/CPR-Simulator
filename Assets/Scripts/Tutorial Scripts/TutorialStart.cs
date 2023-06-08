using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialStart : MonoBehaviour
{
    [SerializeField] UnityEvent _eventsToRun;
    [SerializeField] float _delayToExecuteEvent = .5f;


    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(_delayToExecuteEvent);
        _eventsToRun.Invoke();
    }

}
