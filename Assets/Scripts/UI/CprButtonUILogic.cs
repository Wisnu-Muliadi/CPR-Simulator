using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardiacPatient;

namespace UserInterface
{
    public class CprButtonUILogic : MonoBehaviour
    {
        private Button _cprButton;
        [HideInInspector]
        public Patient patient;
        CardiacPatientLogic _logic;

        [SerializeField] private Slider _pushDepthBar, _bpmBar;
        [SerializeField] private BpmUI _bpmUIScript;

        float _hitBpm;
        float _pushDepth;
        float _sum;

        void Awake()
        {
            _cprButton = GetComponent<Button>();
            _cprButton.onClick.AddListener(HitCpr);
        }
        void OnEnable()
        {
            _cprButton.Select();
        }
        public void HitCpr()
        {
            if (_logic == null) return;

            switch (_pushDepthBar.value)
            {
                case 1:
                    _pushDepth = .2f;
                    break;
                case 3:
                    _logic.HurtPatient(.5f);
                    _pushDepth = .5f * _pushDepthBar.value;
                    break;
                default:
                    _pushDepth = .5f * _pushDepthBar.value;
                    break;
            }
            _hitBpm = 0.008f * Mathf.PingPong(_bpmUIScript.GetBPM(), 120f);
            _sum =  Mathf.Clamp01(_pushDepth * _hitBpm);

            _logic.HandleResuscitation(_sum, _pushDepth);
        }
        public void SetPatient(Patient patient)
        {
            this.patient = patient; // unused. just in case needed
            _logic = patient.cardiacPatientLogic;
        }
    }
}