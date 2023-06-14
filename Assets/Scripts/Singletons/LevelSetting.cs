using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetting : MonoBehaviour
{
    public static LevelSetting Instance;

    bool _trainerMode;
    bool _changeMusic;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(this);
    }
    public void TrainerMode(bool enable) => _trainerMode = enable;
    public bool TrainerMode() => _trainerMode;

    public void MusicChange(bool enable) => _changeMusic = enable;
    public bool MusicChange() => _changeMusic;
}
