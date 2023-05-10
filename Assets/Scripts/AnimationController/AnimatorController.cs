using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardiacPatient;

namespace AnimationController
{
    public abstract class AnimatorController : MonoBehaviour
    {
        public Animator animator;
        protected CardiacPatientAnimator _cardiacPatientAnimator;

        void Start()
        {
            _cardiacPatientAnimator = GetComponentInParent<CardiacPatientAnimator>();
        }
        // Noob: Virtual supaya turunan script ini bisa override
        public virtual void SetTrue(string parameter)
        {
            animator.SetBool(parameter, true);
        }
        public virtual void SetFalse(string parameter)
        {
            animator.SetBool(parameter, false);
        }
        public void PatientAnimatorState(string stateName)
        {
            switch (stateName)
            {
                case nameof(CardiacPatientAnimator.PatientState.Fallen):
                    _cardiacPatientAnimator.patientState = CardiacPatientAnimator.PatientState.Fallen;
                    break;
                case nameof(CardiacPatientAnimator.PatientState.Gasping):
                    _cardiacPatientAnimator.patientState = CardiacPatientAnimator.PatientState.Gasping;
                    break;
                case nameof(CardiacPatientAnimator.PatientState.Saved):
                    _cardiacPatientAnimator.patientState = CardiacPatientAnimator.PatientState.Saved;
                    break;
                case nameof(CardiacPatientAnimator.PatientState.Standing):
                    _cardiacPatientAnimator.patientState = CardiacPatientAnimator.PatientState.Standing;
                    break;
                case nameof(CardiacPatientAnimator.PatientState.Suffocating):
                    _cardiacPatientAnimator.patientState = CardiacPatientAnimator.PatientState.Suffocating;
                    break;
                default:
                    break;
            }
        }
    }
}