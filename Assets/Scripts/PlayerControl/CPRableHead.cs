using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class CPRableHead : MonoBehaviour, ICPRable
    {
        public void Interact(CPRMainManager playerCam)
        {
            playerCam.camState = CPRMainManager.CamState.Head;
            SwapToBreath();

            if (TryGetComponent(out FinishTaskListener task))
                task.FinishTask();
        }
        public string GetDescription()
        {
            return "Periksa Nafas";
        }
        public void SwapToBreath()
        {
            gameObject.AddComponent<CPRableHeadBreath>();
            Destroy(this);
        }
    }
}