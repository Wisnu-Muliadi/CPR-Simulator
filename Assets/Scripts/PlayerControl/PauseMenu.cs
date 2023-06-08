using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] UnityEvent<bool> _pauseEvents;
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
        Time.timeScale = 1;
        _pauseEvents.Invoke(false);
        _uimanager.MouseDisplayAdd(false);
    }
    public void Pause()
    {
        _paused = true;
        Time.timeScale = 0;
        _pauseEvents.Invoke(true);
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

}
