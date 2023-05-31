using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpVolume : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField, Tooltip("in seconds")] float _lerpDuration = .5f;

    float value;
    void Start()
    {
        value = _audioSource.volume;
    }
    public void LerpSound(float targetVolume)
    {
        StopAllCoroutines();
        StartCoroutine(ILerpVolume(targetVolume, _lerpDuration));
    }
    IEnumerator ILerpVolume(float volumeTarget, float duration)
    {
        while (_audioSource.volume != volumeTarget)
        {
            value = Mathf.MoveTowards(value, volumeTarget, duration * Time.deltaTime);
            _audioSource.volume = value;
            yield return null;
        }
    }
}
