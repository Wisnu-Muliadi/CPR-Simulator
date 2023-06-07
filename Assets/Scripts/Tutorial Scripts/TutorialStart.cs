using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;

public class TutorialStart : MonoBehaviour
{
    [SerializeField] PuppetMaster _puppetMaster;
    [SerializeField] float _delayPuppetMasterUnpin = .5f;


    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(_delayPuppetMasterUnpin);
        _puppetMaster.pinWeight = 0;
    }

}
