using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimObject : MonoBehaviour
{
    [SerializeField] Transform _aimTracker;
    public Transform TargetObject;
    [SerializeField, Range(.01f,.1f)] float _trackSpeed = .1f;

    void FixedUpdate()
    {
        if (_aimTracker.position == TargetObject.position) return;
        _aimTracker.position = Vector3.MoveTowards(_aimTracker.position, TargetObject.position, _trackSpeed);
    }
}
