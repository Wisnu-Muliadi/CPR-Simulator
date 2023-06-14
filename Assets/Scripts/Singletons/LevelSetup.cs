using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelSetup : MonoBehaviour
{
    LevelSetting levelSetting;
    public static LevelSetup Instance;

    public bool TrainerMode = false;
    public bool HandsOnly = false;
    public bool MusicChange = false;
    [Header("TrainerMode")]
    [SerializeField] CardiacPatient.CardiacPatientStats _patientStats;
    [SerializeField] AudioSource[] _audioPlayers;
    [SerializeField] UnityEvent _trainerModeEvents = new();

    [Header("Changed Music 103BPM")]
    [SerializeField] AudioClip _stayinAliveMusic;
    [SerializeField] SyncMusicBPM _syncMusicBpm;
    [SerializeField] UnityEvent _changedMusic;

    [Header("Debugs")]
    [SerializeField] bool _debugging;
    [Space]
    [SerializeField] bool _trainerMode;
    [SerializeField] bool _changeMusic;
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        levelSetting = LevelSetting.Instance;

        if (_debugging)
        {
            CheckTrainerMode(_trainerMode);
            CheckMusicChange(_changeMusic);
        }
        else
        {
            if (levelSetting == null) return;
            CheckTrainerMode(levelSetting.TrainerMode());
            CheckMusicChange(levelSetting.MusicChange());
        }
    }
    void CheckTrainerMode(bool enable)
    {
        if (enable)
        {
            _patientStats.maxSuffocationSpeed = 0;
            foreach (AudioSource audioSource in _audioPlayers)
            { audioSource.enabled = false; }

            _trainerModeEvents.Invoke();

            TrainerMode = true;
            CheckHandsOnlyMode(enable);
        }
    }
    void CheckHandsOnlyMode(bool enable)
    {
        if (enable)
        {
            HandsOnly = true;
        }
    }
    void CheckMusicChange(bool enable)
    {
        if (enable)
        {
            _syncMusicBpm.ChangeMusic(_stayinAliveMusic);
            MusicChange = true;
            _changedMusic.Invoke();
        }
    }
}
