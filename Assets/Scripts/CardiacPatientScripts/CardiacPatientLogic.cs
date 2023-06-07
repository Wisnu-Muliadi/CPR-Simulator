using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CardiacPatient
{
    [RequireComponent(typeof(CardiacPatientStats))]
    public class CardiacPatientLogic : MonoBehaviour
    {
        CardiacPatientStats _patientStats;
        [SerializeField]
        Rigidbody _chestRb;
        [SerializeField] UnityEvent _pushChestEvent;

        private float _timer;
        private bool _givingBreath = false;

        void Start()
        {
            _patientStats = GetComponent<CardiacPatientStats>();
        }
        public void HandleResuscitation(float effectiveness, float pushDepth)
        {
            StartCoroutine(IPushChest(.2f * pushDepth, pushDepth));
            _patientStats.Resuscitate(effectiveness);
        }
        public void HandleGiveOxygen(float effectiveness, bool niceBonus)
        {
            if (_givingBreath) return;
            //StartCoroutine(IGiveBreath(.8f));
            _patientStats.GiveOxygen(effectiveness, niceBonus);
        }
        public void HurtPatient(float point)
        {
            _patientStats.patientHealth -= point;
        }
        private IEnumerator IPushChest(float time, float effectiveness)
        {
            _pushChestEvent.Invoke();
            _timer = 0;
            while (_timer < time)
            {
                _timer += Time.deltaTime;
                _chestRb.AddForce(3 * effectiveness * Vector3.down, ForceMode.VelocityChange);
                yield return null;
            }
        }
        /*
        private IEnumerator IGiveBreath(float time)
        {
            _givingBreath = true;
            _timer = 0;
            while (_timer < time)
            {
                _timer += Time.deltaTime;
                _chestRb.AddRelativeTorque(20 * Vector3.left, ForceMode.VelocityChange);
                yield return null;
            }
            _givingBreath = false;
        }*/
    }
}