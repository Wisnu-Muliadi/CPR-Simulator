using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettingButton : ButtonSwitch
{
    [SerializeField] bool _resetAtStart = true;
    LevelSetting levelSetting;
    void Start()
    {
        levelSetting = LevelSetting.Instance;
        if(_resetAtStart)
        {
            LevelSetting.Instance.TrainerMode(false);
            LevelSetting.Instance.MusicChange(false);
        }
    }
    new public void Toggle()
    {
        if (_currentModeA)
        {
            LevelSetting.Instance.MusicChange(true);
            _modeB.Invoke();
        }
        else
        {
            LevelSetting.Instance.MusicChange(false);
            _modeA.Invoke();
        }
        _currentModeA = !_currentModeA;
    }
    public void TrainerModeSetting()
    {
        levelSetting.TrainerMode(true);
    }
}
