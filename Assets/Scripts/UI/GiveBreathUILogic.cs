using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using CardiacPatient;
#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

namespace UserInterface
{
    public class GiveBreathUILogic : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Button _breathButton;
        [HideInInspector]
        public Patient patient;
        CardiacPatientLogic _logic;
        [SerializeField] bool _isMobile = false;

        [SerializeField] Slider _breathBar;
        [SerializeField] TextMeshProUGUI _percentText;
        [SerializeField] Animator _popUpAnimator;
        float _greenBlue = 0;
        [SerializeField] float _delayForEachGive = 3f;
        private Image _fillArea;

        private float _timer = 0;
        private readonly float _breathSpeed = 1;
        private bool _count, _nice;
        [SerializeField] private UnityEvent _pointerDown, _gaveBreath;
        public UnityEvent<bool> PointerEnterEvent = new();

        void Awake()
        {
            _breathButton = GetComponent<Button>();
            _breathButton.onClick.AddListener(Evaluate);
            _count = false;
            _fillArea = _breathBar.fillRect.GetComponent<Image>();
        }
        void OnEnable()
        {
            _breathBar.value = 0;
        }
        void Update()
        {
            if (_count)
            {
                _timer += Time.deltaTime * _breathSpeed;
                _breathBar.value = Mathf.PingPong(_timer, 1f);
                _greenBlue = _breathBar.value;
                _percentText.text = (100f * _breathBar.value).ToString("0") + "%";
                _fillArea.color = new Color(1f - _greenBlue, _greenBlue, 0f);
            }
        }
#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern bool IsMobile();
#endif
        public void OnPointerDown(PointerEventData eventData)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            _isMobile = IsMobile();
#endif
            if(_isMobile)
            {
                if(Input.touchCount > 0)
                {
                    if(Input.GetTouch(0).phase != TouchPhase.Began)
                    {
                        return;
                    }
                }
            }
            _pointerDown.Invoke();
            _popUpAnimator.Play("Hide");
            _timer = 0;
            _count = true;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnterEvent.Invoke(true);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            PointerEnterEvent.Invoke(false);
        }
        public void Evaluate()
        {
            _nice = false;
            _count = false;
            if (_breathBar.value > Random.Range(.95f, .98f))
            {
                _popUpAnimator.Play("PopUp");
                _breathBar.value = 1;
                _percentText.text = "100%";
                _nice = true;
            }
            StartCoroutine(IDelayNextGive(_delayForEachGive));
            _logic.HandleGiveOxygen(_breathBar.value, _nice);
            _gaveBreath?.Invoke();
        }
        private IEnumerator IDelayNextGive(float duration)
        {
            ButtonScriptActive(false);
            float timer = 0;
            while (timer <= duration)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            ButtonScriptActive(true);
        }
        public void ButtonScriptActive(bool activate)
        {
            _breathButton.enabled = activate;
            enabled = activate;
            _count = false;
        }
        public void SetPatient(Patient patient)
        {
            this.patient = patient; // unused. just in case needed
            _logic = patient.cardiacPatientLogic;
        }

    }
}