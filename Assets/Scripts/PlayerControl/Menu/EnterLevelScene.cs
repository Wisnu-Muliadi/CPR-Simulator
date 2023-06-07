using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterLevelScene : MonoBehaviour
{
    public int TargetGameSceneIndex = 2;
    public void EnterGameScene()
    {
        SceneManager.LoadScene(TargetGameSceneIndex);
    }
    public void GoToScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
