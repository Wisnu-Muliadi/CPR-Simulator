using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class CPRableArm : MonoBehaviour, ICPRable
    {
        private Rigidbody _rb;
        
        private bool _shake;
        private float _timer = 0;
        private float _force = 0;
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
        void FixedUpdate()
        {
            if (_shake)
            {
                _rb.AddRelativeForce(6f * Mathf.Sin(_force * 360f * 4f) * Vector3.right, ForceMode.Impulse);
            }
        }
        private IEnumerator IShakingPatient(float duration)
        {
            _shake = true;
            for (_timer = 0 ; _timer < duration ; _timer += Time.deltaTime)
            {
                _force = _timer / duration;
                yield return null;
            }
            _shake = false;
        }
    }
}