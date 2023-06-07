using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserInterface;

public class SyncMusicBPM : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField] AudioClip _music;
    [SerializeField] BpmUI _bpmUI;
    [SerializeField] Animation _animation;

    void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.bypassEffects = true;
        _audioSource.bypassReverbZones = true;
        enabled = false;
    }
    void Update()
    {
        if (_audioSource.isPlaying)
        {
            _bpmUI.BpmTimer = _audioSource.time;
            _bpmUI.enabled = true;
            _animation.Play();
            enabled = false;
        }
    }
    public void SyncStart()
    {
        _audioSource.PlayOneShot(_music);
        enabled = true;
    }
}
