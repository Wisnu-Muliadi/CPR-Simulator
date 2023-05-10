using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInstance : MonoBehaviour
{
    public static GlobalInstance Instance { get; private set; }

    public UIManager UIManager { get; private set; }
    public TaskManager TaskManager { get; private set; }

    public Camera mainCam;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        UIManager = GetComponentInChildren<UIManager>();
        TaskManager = GetComponentInChildren<TaskManager>();
    }

}
