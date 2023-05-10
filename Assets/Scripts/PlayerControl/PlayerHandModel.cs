using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using CardiacPatient;

namespace PlayerControl
{
    public class PlayerHandModel : MonoBehaviour
    {
        [HideInInspector]
        public Patient patient;
        [SerializeField]
        private Transform _skeletonCamProxy, _skeletonRoot, _playerCamera;
        [SerializeField]
        private Animator _modelAnimator;
        [SerializeField]
        private CPRMainManager _cprManager;
        [SerializeField]
        private SkinnedMeshRenderer _handMesh;

        [Tooltip("Order of Array Matters [0]Left, [1] Right")]
        public Transform[] chestIkRig, headIkRig, chestIkTarget, headIkTarget;
        private int _i;
        private Rig _rigChestIK, _rigHeadIK;

        private bool _cprEnabled, _mirrorAnim;

        void Start()
        {
            _skeletonRoot.SetParent(_skeletonCamProxy, true);
            _rigChestIK = GetComponent<RigBuilder>().layers[0].rig;
            _rigHeadIK = GetComponent<RigBuilder>().layers[1].rig;
        }
        void Update()
        {
            _skeletonCamProxy.SetPositionAndRotation(_playerCamera.position, _playerCamera.rotation);
            switch (_cprManager.camState)
            {
                case CPRMainManager.CamState.Chest:
                    for (_i = 0; _i < chestIkRig.Length; _i++)
                    {
                        chestIkRig[_i].SetPositionAndRotation(chestIkTarget[_i].position, chestIkTarget[_i].rotation);
                    }
                    break;
                case CPRMainManager.CamState.BreathSupport:
                    for (_i = 0; _i < headIkRig.Length; _i++)
                    {
                        headIkRig[_i].SetPositionAndRotation(headIkTarget[_i].position, headIkTarget[_i].rotation);
                    }
                    break;
            }
        }
        void LateUpdate()
        {
            if (_modelAnimator.IsInTransition(0))
            {
                _modelAnimator.ResetTrigger("reset");
                _modelAnimator.ResetTrigger("cpr");
            }
            if (_cprManager.isActiveAndEnabled)
            {
                if (_cprManager.StateChanged())
                {
                    switch (_cprManager.camState)
                    {
                        case CPRMainManager.CamState.Overview:
                            _modelAnimator.SetFloat("Speed", 1f);
                            _modelAnimator.Play("CPR_IdleStart", 0, 0f);
                            // Disable IK Rigs
                            StopAllCoroutines();
                            _rigChestIK.weight = 0;
                            _rigHeadIK.weight = 0;
                            break;
                        case CPRMainManager.CamState.Head:
                            _modelAnimator.SetFloat("Speed", 0f);
                            _modelAnimator.CrossFadeInFixedTime("CPR_IdleStart", 1f);
                            break;
                        case CPRMainManager.CamState.Chest:
                            _modelAnimator.CrossFadeInFixedTime("Positioning", .25f);
                            break;
                        case CPRMainManager.CamState.BreathSupport:
                            if(_mirrorAnim) _modelAnimator.Play("HeadBreath Mirror");
                            else _modelAnimator.Play("HeadBreath");
                            StartCoroutine(IBlendInIKHead(.6f));
                            break;
                    }
                }
                if (!_cprEnabled)
                {
                    // happens when start attending patient
                    _modelAnimator.SetFloat("Speed", 1f);
                    _cprEnabled = true;
                    _handMesh.enabled = true;
                    _rigChestIK.weight = 0;
                    _rigHeadIK.weight = 0;
                }
            }
            else if (_cprEnabled)
            {
                _modelAnimator.SetFloat("Speed", 0f);
                _modelAnimator.Play("CPR_IdleStart", 0, 0f);
                _cprEnabled = false;
                _handMesh.enabled = false;
                _rigChestIK.weight = 0;
            }
        }

        public void MirroredAnim(bool mirror)
        {
            _mirrorAnim = mirror;
        }
        public void BlendInIKChest(float duration)
        {
            StartCoroutine(IBlendInIKChest(duration));
        }
        private IEnumerator IBlendInIKChest(float duration)
        {
            _rigChestIK.weight = 0;
            while (_rigChestIK.weight < 1)
            {
                _rigChestIK.weight = Mathf.MoveTowards(_rigChestIK.weight, 1, Time.deltaTime/duration);
                yield return null;
            }
        }
        private IEnumerator IBlendInIKHead(float duration)
        {
            _rigHeadIK.weight = 0;
            while (_rigHeadIK.weight < .98f)
            {
                _rigHeadIK.weight = Mathf.Lerp(_rigHeadIK.weight, 1, Time.deltaTime / duration);
                yield return null;
            }
            _rigHeadIK.weight = 1;

        }
    }
}