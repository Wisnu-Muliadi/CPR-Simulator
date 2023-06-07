using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerControl
{
    public class CPRableHeadBreath : MonoBehaviour, ICPRable
    {
        public UnityAction InteractAction { get; set; }
        public void Interact(CPRMainManager playerCam)
        {
            playerCam.camState = CPRMainManager.CamState.BreathSupport;
            InteractAction?.Invoke();
        }
        public string GetDescription()
        {
            return "Beri Nafas Buatan";
        }
    }
}