using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardiacPatient;

namespace AnimationController
{
    public class AkioAnimatorController : AnimatorController
    {
        public Animator akioAnimator;
        PuppetMasterController _puppetController;
        bool updated = false;

        private void Start()
        {
            animator = akioAnimator;
            _cardiacPatientAnimator = GetComponentInParent<CardiacPatientAnimator>();
            _puppetController = GetComponent<PuppetMasterController>();
        }
        private void Update()
        {
            if (akioAnimator.IsInTransition(0))
            {
                if (updated) return;
                updated = true;
                _puppetController?.UpdateJointAnchors(2f);
            }
            else
                updated = false;
        }
    }
}