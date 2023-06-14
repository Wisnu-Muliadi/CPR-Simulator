using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] bool _stopTime = true;
    [SerializeField] UnityEvent _pauseEvents;
    [SerializeField] UnityEvent _unPauseEvents;
    UIManager _uimanager;
    bool _paused = false;

    private void Start()
    {
        _uimanager = GlobalInstance.Instance.UIManager;
    }
    void Update()
    {
        if (Input.GetButtonDown("Pause Menu"))
        {
            if (_paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        _paused = false;
        if(_stopTime)Time.timeScale = 1;
        _unPauseEvents.Invoke();
        _uimanager.MouseDisplayAdd(false);
    }
    public void Pause()
    {
        _paused = true;
        if(_stopTime)Time.timeScale = 0;
        _pauseEvents.Invoke();
        _uimanager.MouseDisplayAdd(true);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void GoToScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

}
