using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace CardiacPatient
{
    [RequireComponent(typeof(CardiacPatientStats))]
    public class CardiacPatientAnimator : MonoBehaviour
    {
        [Header("*Prerequisites*")]
        [SerializeField, Tooltip("Animator Needs Parameters:\n" +
            "Gasp(Trigger), Suffocate(Bool), Attended(Bool), Saved(Trigger), Speed (Float)")]
        private Animator _animatorController;
        private CardiacPatientStats _patientStats;
        private readonly WaitForEndOfFrame endOfFrame = new();

        #region System Serializable
        [System.Serializable]
        public enum PatientState { 
            Standing,
            Fallen,
            Gasping,
            Suffocating,
            Saved
        }
        [System.Serializable]
        private struct SendEvent {
            public UnityEvent e_EnteredStanding,
                e_EnteredFallen,
                e_EnteredGasping,
                e_EnteredSuffocating,
                e_EnteredSaved;
        }
        #endregion

        public PatientState patientState;
        private PatientState _prevPatientState;

        [SerializeField]
        private SendEvent _stateChangeEvent;

        [Header("Parameters")]
        [SerializeField, Tooltip("BrainOxygenLevel when Patient Begins Gasping")]
        private float _gaspHealthLevel = 90f;
        [SerializeField, Tooltip("Seconds Before Every Gasps")]
        private float _gaspingPeriod = 8f;
        bool _gaspInvoked = false;
        [SerializeField, Tooltip("BrainOxygenLevel when Patient is Suffocating")]
        private float _suffocatedHealthLevel = 65f;


        void Start()
        {
            _patientStats = GetComponent<CardiacPatientStats>();
            _prevPatientState = patientState;
        }

        // Update is called once per frame
        void Update()
        {
            switch (patientState)
            {
                case PatientState.Standing:
                    break;
                case PatientState.Fallen:
                    _patientStats.enabled = true;
                    CheckForGasp();
                    break;
                case PatientState.Gasping:
                    if(!_gaspInvoked)
                    {
                        InvokeRepeating(nameof(AnimatorGasp), 0, _gaspingPeriod);
                        _gaspInvoked = true;
                    }
                    CheckForSuffocate();
                    break;
                case PatientState.Suffocating:
                    if (!_gaspInvoked)
                    {
                        InvokeRepeating(nameof(AnimatorGasp), 0, 2 * _gaspingPeriod);
                        _gaspInvoked = true;
                    }
                    break;
                case PatientState.Saved:
                    _animatorController.SetTrigger("Saved");
                    patientState = PatientState.Standing;
                    if (_gaspInvoked)
                    {
                        CancelInvoke(nameof(AnimatorGasp));
                        _gaspInvoked = false;
                    }
                    break;
                default:
                    Debug.Log("Cardiac Patient Animator State: Something Broke Here?");
                    break;
            }
        }
        private void CheckForGasp()
        {
            if (_patientStats.patientBrainOxygenLevel < _gaspHealthLevel)
            {
                _animatorController.SetTrigger("Gasp");
                patientState = PatientState.Gasping;
            }
        }
        private void CheckForSuffocate()
        {
            if (_patientStats.patientBrainOxygenLevel < _suffocatedHealthLevel)
            {
                _animatorController.SetBool("Suffocate", true);
                _animatorController.SetFloat("Speed", 0.1f);
                patientState = PatientState.Suffocating;
                CancelInvoke(nameof(AnimatorGasp));
                _gaspInvoked = false;
            }
        }
        public void AnimatorGasp()
        {
            _animatorController.SetTrigger("Gasp");
            StartCoroutine(nameof(ResetGasp));
        }
        private IEnumerator ResetGasp()
        {
            yield return endOfFrame;
            _animatorController.ResetTrigger("Gasp");
        }

        private void LateUpdate()
        {
            SendEvents();
        }
        private void SendEvents()
        {
            if (_prevPatientState != patientState)
            {
                switch (patientState)
                {
                    case PatientState.Standing:
                        _stateChangeEvent.e_EnteredStanding.Invoke();
                        break;
                    case PatientState.Fallen:
                        _stateChangeEvent.e_EnteredFallen.Invoke();
                        break;
                    case PatientState.Gasping:
                        _stateChangeEvent.e_EnteredGasping.Invoke();
                        break;
                    case PatientState.Suffocating:
                        _stateChangeEvent.e_EnteredSuffocating.Invoke();
                        break;
                    case PatientState.Saved:
                        _stateChangeEvent.e_EnteredSaved.Invoke();
                        break;
                }
            }
            _prevPatientState = patientState;
        }
    }
}