using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterLevelScene : MonoBehaviour
{
    public void EnterGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
