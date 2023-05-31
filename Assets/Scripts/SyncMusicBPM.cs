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

    void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.bypassEffects = true;
        _audioSource.bypassReverbZones = true;
    }
    public void SyncStart()
    {
        _audioSource.PlayOneShot(_music);
        _bpmUI.BpmTimer = _audioSource.time;
        _bpmUI.enabled = true;
        if (_animation != null)
            _animation.Play();
    }
}
