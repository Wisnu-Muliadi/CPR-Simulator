using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class CPRableArm : MonoBehaviour, ICPRable
    {
        private Rigidbody _rb;
        public void Interact(CPRMainManager playerCam)
        {
            _rb = GetComponent<Rigidbody>();
            StartCoroutine(IShakingPatient(.64f));
            
            if (TryGetComponent(out FinishTaskListener task))
                task.FinishTask();
        }
        public string GetDescription()
        {
            return "Guncang";
        }

        private IEnumerator IShakingPatient(float duration)
        {
            for (float timer = 0 ; timer < duration ; timer += Time.deltaTime)
            {
                _rb.AddRelativeForce(6f * Mathf.Sin(timer/duration * 360f * 4f) * Vector3.right, ForceMode.Impulse);
                yield return null;
            }
        }
    }
}