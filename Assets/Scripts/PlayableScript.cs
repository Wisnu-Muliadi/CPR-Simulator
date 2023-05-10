using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayableScript : MonoBehaviour
{
    private PlayableDirector _director;
    private GameObject _controlObject;

    void Start()
    {
        _director = GetComponent<PlayableDirector>();
    }
    public void PlayDirector()
    {
        _director.Play();
    }
}
