using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CardiacPatient
{
    public class CardiacPatientStats : MonoBehaviour
    {
        [Header("Stats (Do Not Edit)")]
        public float patientHealth = 100;
        public float patientBrainOxygenLevel = 100; // berkurang berdasarkan suffocationSpeed
        private float _healthStart, _oxygenStart;
        public float LowBrainOxygenThreshold { get; private set; } = 40; // oksigen otak kurang dari nilai ini maka akan mulai mengurangi patientHealth dengan kecepatan patientBrainOxygenLevel

        public float SuffocationSpeed { get; private set; } = 0; // selalu meningkat. Hanya berkurang jika player kompresi dada * tingkat oxygen darah
        [SerializeField] float _bloodOxygenLevel = 2; // berkurang seiring kompresi dada

        [Header("Parameters")]
        public float maxSuffocationSpeed = .75f;
        const float _maxBloodOxygenLevel = 2;
        const float _suffocationSpeedIncrement = 0.2f;
        const float _bloodOxygenReductionIncrement = 0.08f;
        [Space]
        [SerializeField] UnityEvent onZeroHealth;

        void Start()
        {
            enabled = false;
            _healthStart = patientHealth;
            _oxygenStart = patientBrainOxygenLevel;
        }
        void FixedUpdate()
        {
            PatientSuffocating();
            patientBrainOxygenLevel -= SuffocationSpeed * Time.deltaTime;

            if(patientBrainOxygenLevel < LowBrainOxygenThreshold && patientHealth > 0)
            {
                PatientHealthDecreasing();
                if (patientHealth < 0) onZeroHealth?.Invoke();
            }
        }
        private void PatientSuffocating()
        {
            // Blood Oxygen Reduction
            if(SuffocationSpeed < maxSuffocationSpeed)
            {
                // More Buffer Time when Patient not Suffocating
                if (SuffocationSpeed < 0)
                {
                    SuffocationSpeed += .1f * _suffocationSpeedIncrement * Time.deltaTime;
                }
                else
                {
                    SuffocationSpeed += _suffocationSpeedIncrement * Time.deltaTime;
                }
            }
        }
        private void PatientHealthDecreasing() => patientHealth -= (_healthStart - patientBrainOxygenLevel) / 50 * Time.deltaTime;
        public float PatientHealthPercentage() => patientHealth / _healthStart;
        public float PatientOxygenPercentage() => patientBrainOxygenLevel / _oxygenStart;

        /// <summary>
        /// Effectiveness of Resuscitation. Maximum 1
        /// </summary>
        /// <param name="pointMultiplier"></param>
        public void Resuscitate(float pointMultiplier)
        {
            // Cannot resuscitate If Blood Oxygen Level too low
            if (_bloodOxygenLevel < .1f) return;

            SuffocationSpeed -= .2f * _bloodOxygenLevel * pointMultiplier;
            SuffocationSpeed = Mathf.Clamp(SuffocationSpeed, -.05f, maxSuffocationSpeed);
            _bloodOxygenLevel -= _bloodOxygenReductionIncrement * pointMultiplier;
        }
        /// <summary>
        /// Effectiveness of Giving Breath. Maximum 1
        /// </summary>
        /// <param name="pointMultiplier"></param>
        public void GiveOxygen(float pointMultiplier, bool niceBonus)
        {
            // Kali .5 supaya harus dua kali beri napas
            _bloodOxygenLevel += 0.5f * _maxBloodOxygenLevel * pointMultiplier;
            _bloodOxygenLevel = Mathf.Clamp(_bloodOxygenLevel, 0, _maxBloodOxygenLevel);
            if (_bloodOxygenLevel < _maxBloodOxygenLevel)
            {
                if(niceBonus)
                    patientBrainOxygenLevel += 2 * pointMultiplier;
                SuffocationSpeed -= .5f * pointMultiplier;
                SuffocationSpeed = Mathf.Clamp(SuffocationSpeed, -.05f, maxSuffocationSpeed);
            }
        }
    }
}