using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;

namespace AnimationController
{
    public class PuppetMasterController : MonoBehaviour
    {
        public PuppetMaster puppetMaster;
        public CardiacPatient.CardiacPatientMuscles muscles;
        //**//
        public void KinematicOff()
        {
            puppetMaster.SwitchToActiveMode();
        }
        //**//
        public void UpdateJointAnchors(float duration)
        {
            WaitForSeconds waitDuration = new(duration);
            StartCoroutine(nameof(IUpdateJointAnchors), value: waitDuration);
        }
        IEnumerator IUpdateJointAnchors(WaitForSeconds waitDuration)
        {
            puppetMaster.updateJointAnchors = true;
            yield return waitDuration;
            puppetMaster.updateJointAnchors = false;
        }
        //**//
        public void AngularLimitsOff()
        {
            puppetMaster.angularLimits = false;
        }
        //**//
        public void MuscleTwitch(int handorlegs)
        {
            if (muscles == null) return;
            switch (handorlegs) {
                case 1:
                    foreach (Muscle muscle in muscles.handMuscles)
                    {
                        muscle.props.muscleWeight = .1f;
                    }
                    break;
                case 2:
                    foreach (Muscle muscle in muscles.legMuscles)
                    {
                        muscle.props.muscleWeight = .1f;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}