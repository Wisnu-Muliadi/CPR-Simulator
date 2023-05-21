using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class CPRableHead : MonoBehaviour, ICPRable
    {
        WaitForSecondsRealtime waitASec = new(1.5f);
        public void Interact(CPRMainManager playerCam)
        {
            playerCam.camState = CPRMainManager.CamState.Head;

            StartCoroutine(IDelayFinishTask());
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
        private IEnumerator IDelayFinishTask()
        {
            yield return waitASec;
            GetComponentInParent<CardiacPatient.CardiacPatientAnimator>().AnimatorGasp(); //expensivee ?
            yield return waitASec;
            if (TryGetComponent(out FinishTaskListener task))
                task.FinishTask();
            SwapToBreath();
            try
            {
                GlobalInstance.Instance.UIManager.loadingCircle.SetActive(false);
                GlobalInstance.Instance.UIManager.captionPool.EnqueueCaption("Dia <b>kesulitan bernafas!</b>", 3f);
            }
            catch { }
        }

    }
}