using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserInterface;

public class SyncMusicBPM : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _music;
    [SerializeField] BpmUI _bpmUI;
    [SerializeField] Animation _animation;
    Slider bpmSlider;
    bool noAudioMode = false;
    bool halfUp = false;
    //WaitForSecondsRealtime waitABit = new(1);

    void Awake()
    {
        if(_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.bypassEffects = true;
        _audioSource.bypassReverbZones = true;
        enabled = false;
        bpmSlider = _bpmUI.GetBpmSlider();
    }
    void Update()
    {
        switch (halfUp)
        {
            case true:
                if (bpmSlider.value < .04f) break;
                else _animation.Play(); halfUp = false;
                break;
            case false:
                if (bpmSlider.value > .96f) break;
                else _animation.Play(); halfUp = true;
                break;
        }
    }
    public void SyncStart()
    {
        _audioSource.PlayOneShot(_music);
        StartCoroutine(SetupAudio());
        //waitABit = new(120 / _bpmUI.TargetBPM);
        enabled = true;
    }
    IEnumerator SetupAudio()
    {
        yield return new WaitUntil(() => _audioSource.isPlaying || noAudioMode);
        _bpmUI.BpmTimer = _audioSource.time;
        _bpmUI.enabled = true;
        /*while (_audioSource.isPlaying)
        {
            yield return waitABit;
            _bpmUI.BpmTimer = 0;
        }*/
    }
    public void NoAudioPlay() => noAudioMode = true;
    public void ChangeMusic(AudioClip newMusic) => _music = newMusic;
}
