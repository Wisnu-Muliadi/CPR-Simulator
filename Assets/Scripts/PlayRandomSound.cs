using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] List<AudioClip> _audioClips;
    [SerializeField, Range(0,1)] float _volume = 1;
    void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.spatialBlend = 1;
    }
    public void PlayRandom()
    {
        int random = Random.Range(0, _audioClips.Count);
        _audioSource.PlayOneShot(_audioClips[random], _volume);
    }
}
