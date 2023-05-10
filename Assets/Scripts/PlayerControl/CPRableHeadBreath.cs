using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class CPRableHeadBreath : MonoBehaviour, ICPRable
    {
        public void Interact(CPRMainManager playerCam)
        {
            playerCam.camState = CPRMainManager.CamState.BreathSupport;
        }
        public string GetDescription()
        {
            return "Beri Nafas Buatan";
        }
    }
}