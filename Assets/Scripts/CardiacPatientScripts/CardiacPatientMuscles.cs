using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;

namespace CardiacPatient
{
    [RequireComponent(typeof(CardiacPatientStats))]
    public class CardiacPatientMuscles : MonoBehaviour
    {
        [Header("*Prerequisites*")]
        [SerializeField]
        private PuppetMaster _puppetMaster;
        public Muscle[] handMuscles, legMuscles;
        private CardiacPatientStats _patientStats;

        [Header("Parameters")]
        [SerializeField] private float _musclesOnAttend = 1f;
        [SerializeField, Range(0f, 1f)] private float _minimumMuscleWeight = .05f;
        [SerializeField, Range(0f, 1f)] private float _maximumMuscleWeight = .6f;
        [SerializeField, Tooltip("BrainOxygenLevel value where the Muscle will hit MinimumMuscleWeight")]
        private float _lowHealthThreshold = 40f;
        public bool attended = false;

        private float _initialHealth;
        private float _health;
        public float MusclePower { get; private set; } = 1f;

        private void Start()
        {
            _patientStats = GetComponent<CardiacPatientStats>();
            _initialHealth = _patientStats.patientBrainOxygenLevel - _lowHealthThreshold;
            
            handMuscles = new Muscle[]
            {
                _puppetMaster.muscles[9], 
                _puppetMaster.muscles[10],
                _puppetMaster.muscles[11],
                _puppetMaster.muscles[13],
                _puppetMaster.muscles[14],
                _puppetMaster.muscles[15]
            };
            legMuscles = new Muscle[]
            {
                _puppetMaster.muscles[1],
                _puppetMaster.muscles[2],
                _puppetMaster.muscles[3],
                _puppetMaster.muscles[4],
                _puppetMaster.muscles[5],
                _puppetMaster.muscles[6]
            };

        }
        // Update is called once per frame
        void Update()
        {
            if (attended)
            {
                StartCoroutine(nameof(RaiseMuscleWeight));
                enabled = false;
                return;
            }
            _health = (_patientStats.patientBrainOxygenLevel - _lowHealthThreshold) /_initialHealth * _maximumMuscleWeight;
            MusclePower = Mathf.Clamp(_health, _minimumMuscleWeight, _maximumMuscleWeight);

            //_puppetMaster.muscleWeight = _muscle;
            foreach (Muscle muscle in handMuscles)
            {
                muscle.props.muscleWeight = MusclePower;
            }
            foreach (Muscle muscle in legMuscles)
            {
                muscle.props.muscleWeight = MusclePower;
            }
        }

        private IEnumerator RaiseMuscleWeight()
        {
            if (_health < _lowHealthThreshold)
                yield break;
            foreach (Muscle muscle in handMuscles)
            {
                muscle.props.muscleWeight = _musclesOnAttend;
            }
            foreach (Muscle muscle in legMuscles)
            {
                muscle.props.muscleWeight = _musclesOnAttend;
            }
            float time = 0;
            while (_puppetMaster.muscleWeight < _musclesOnAttend && time < 2)
            {
                time += Time.deltaTime;
                _puppetMaster.muscleWeight = Mathf.MoveTowards(_puppetMaster.muscleWeight, _musclesOnAttend, 0.05f);
                yield return null;
            }
            //enabled = true;
        }

        public void StartAttend()
        {
            attended = true;
        }
        public void StopAttend()
        {
            attended = false;
            enabled = true;
        }
    }
}