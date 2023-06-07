using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerControl
{
    public class CPRableChest : MonoBehaviour, ICPRable
    {
        public UnityAction InteractAction { get; set; }
        public void Interact(CPRMainManager playerCam)
        {
            playerCam.camState = CPRMainManager.CamState.Chest;
            InteractAction?.Invoke();
        }
        public string GetDescription()
        {
            return "CPR";
        }
    }
}