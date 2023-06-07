using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerControl
{
    public class CPRableArm : MonoBehaviour, ICPRable
    {
        public UnityAction InteractAction { get; set; }
        private Rigidbody _rb;
        
        private bool _shake;
        private float _timer = 0;
        private float _force = 0;

        private List<string> _shakeWords = new();
        void Start()
        {
            _shakeWords.Add("Hey! Bisa dengar aku?");
            _shakeWords.Add("Kamu kenapaa ?");
            _shakeWords.Add("Bertahan. Ku panggil bantuan!");
        }
        public void Interact(CPRMainManager playerCam)
        {
            _rb = GetComponent<Rigidbody>();
            StartCoroutine(IShakingPatient(.64f));
            
            if (TryGetComponent(out FinishTaskListener task))
                task.FinishTask();
            if (TryGetComponent(out PlayRandomSound soundPlayer))
                soundPlayer.PlayRandom();
            try
            {
                GlobalInstance.Instance.UIManager.captionPool.EnqueueCaption(_shakeWords[Random.Range(0,_shakeWords.Count-1)], 2f);
            }
            catch { Debug.Log("ArmCheck cannot get CaptionPool Class"); }
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
            InteractAction?.Invoke();
        }
    }
}