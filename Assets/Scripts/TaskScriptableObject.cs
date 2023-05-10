using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TaskScriptableObject", menuName = "ScriptableObjects/CreateNewTaskObject")]
public class TaskScriptableObject : ScriptableObject
{
    public string[] Tasks;
}
