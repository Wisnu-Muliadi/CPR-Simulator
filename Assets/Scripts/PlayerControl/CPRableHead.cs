using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerControl
{
    public class CPRableHead : MonoBehaviour, ICPRable
    {
        public UnityAction InteractAction { get; set; }
        WaitForSecondsRealtime waitASec = new(1.5f);
        public void Interact(CPRMainManager playerCam)
        {
            playerCam.camState = CPRMainManager.CamState.Head;
            try
            {
                StartCoroutine(IDelayFinishTask(LevelSetup.Instance.HandsOnly));
            } catch {
                Debug.LogWarning("no LevelSetup singleton, using old default CPRableHead values");
                StartCoroutine(IDelayFinishTask(false));
            }
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
        private IEnumerator IDelayFinishTask(bool handsOnly)
        {
            yield return waitASec;
            CardiacPatient.CardiacPatientAnimator cAnimator = GetComponentInParent<CardiacPatient.CardiacPatientAnimator>();
            if (cAnimator) cAnimator.AnimatorGasp(); //expensivee ?
            yield return waitASec;
            if (TryGetComponent(out FinishTaskListener task))
                task.FinishTask();
            if (!handsOnly) SwapToBreath();
            else GlobalInstance.Instance.UIManager.bpmUI.ResetUI();
            try
            {
                GlobalInstance.Instance.UIManager.loadingCircle.SetActive(false);
                GlobalInstance.Instance.UIManager.captionPool.EnqueueCaption("Dia <b>kesulitan bernafas!</b>", 3f);
            }
            catch { Debug.LogWarning("HeadCheck Can't get Loading Circle or CaptionPool Class"); }
            InteractAction?.Invoke();
        }

    }
}