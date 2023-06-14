using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CardiacPatient;

namespace UserInterface
{
    public class CprButtonUILogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Button _cprButton;
        private RectTransform _rectTransform;
        [HideInInspector]
        public Patient patient;
        CardiacPatientLogic _logic;

        [SerializeField] Slider _pushDepthBar, _bpmBar;
        [SerializeField] BpmUI _bpmUIScript;
        [SerializeField] Vector3 _growScale;
        private Vector3 _originalScale;

        private float _hitEfficiency;
        private float _pushDepth;
        private float _sum;

        void Awake()
        {
            _cprButton = GetComponent<Button>();
            _cprButton.onClick.AddListener(HitCpr);
            _rectTransform = GetComponent<RectTransform>();
            _originalScale = _rectTransform.localScale;
        }
        void OnEnable()
        {
            _cprButton.Select();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(ButtonHover(true));
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(ButtonHover(false));
        }
        private IEnumerator ButtonHover(bool hover)
        {
            if(hover)
                while(_rectTransform.localScale != _growScale)
                {
                    _rectTransform.localScale = Vector3.MoveTowards(_rectTransform.localScale, _growScale, 2f * Time.deltaTime);
                    yield return null;
                }
            else
                while (_rectTransform.position != _originalScale)
                {
                    _rectTransform.localScale = Vector3.MoveTowards(_rectTransform.localScale, _originalScale, 2f * Time.deltaTime);
                    yield return null;
                }
        }
        public void HitCpr()
        {
            if (_logic == null) return;

            switch (_pushDepthBar.value)
            {
                case var _ when _pushDepthBar.value >= 6:
                    _logic.HurtPatient(.5f);
                    _pushDepth = 1f;
                    break;
                default:
                    _pushDepth = .2f * _pushDepthBar.value;
                    break;
            }
            _hitEfficiency = _bpmUIScript.GetEfficiency();
            _sum =  Mathf.Clamp01(_pushDepth * _hitEfficiency);
            _logic.HandleResuscitation(_sum, _pushDepth);
            //Debug.Log("hit efficiency: " + _hitEfficiency);
            //Debug.Log("send summary: " + _sum);
        }
        public void SetPatient(Patient patient)
        {
            this.patient = patient; // unused. just in case needed
            _logic = patient.cardiacPatientLogic;
        }
    }
}