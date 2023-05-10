using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class CPRableChest : MonoBehaviour, ICPRable
    {
        public void Interact(CPRMainManager playerCam)
        {
            playerCam.camState = CPRMainManager.CamState.Chest;
        }
        public string GetDescription()
        {
            return "CPR";
        }
    }
}