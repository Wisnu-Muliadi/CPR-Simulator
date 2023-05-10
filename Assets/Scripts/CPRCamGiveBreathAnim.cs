using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    //Scrap
    public class CPRCamGiveBreathAnim : MonoBehaviour
    {
        Vector3 _defaultPos;
        Quaternion _defaultRot;
        Vector3 _pullStartPos, _pullEndPos;

        [SerializeField] float _lerpMultiplier = .2f;
        [SerializeField] float _pullDistance = .25f;

        void Start()
        {
            _defaultPos = transform.localPosition;
            _defaultRot = transform.localRotation;
            _pullStartPos = _defaultPos;
            _pullEndPos = _pullStartPos + _pullDistance * Vector3.back;
        }
        void Update()
        {
            StayAtDefault();
        }
        private void StayAtDefault()
        {
            LerpTransformLocal(transform, _defaultPos, _defaultRot, _lerpMultiplier * Time.deltaTime);
        }
        public void Pull(float distance)
        {
            transform.position = Vector3.Lerp(_pullStartPos, _pullEndPos, distance);
        }

        private void LerpTransformLocal(Transform original, Vector3 targetPos, Quaternion targetRot, float time)
        {
            original.SetLocalPositionAndRotation(Vector3.Lerp(original.localPosition, targetPos, time),
                Quaternion.Slerp(original.localRotation, targetRot, time));
            //return original;
        }
    }
}