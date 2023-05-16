using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [SerializeField] Animator _pausedAnimation;

    void Start()
    {
        _pausedAnimation.enabled = false;
    }
    public void FinishIntro()
    {
        _pausedAnimation.enabled = true;
    }
}
