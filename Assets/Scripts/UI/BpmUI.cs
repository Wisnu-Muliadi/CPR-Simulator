using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace UserInterface {
    public class BpmUI : MonoBehaviour
    {
        [SerializeField] bool disableAfterStart = false;
        [SerializeField] private Slider _bpmHelperBar;
        [SerializeField] private Slider _pushDepthBar;
        [SerializeField] private TextMeshProUGUI _bpmUICounter;
        RectTransform _counterTransform;
        [SerializeField] private TextMeshProUGUI _cprPushCounter;
        [SerializeField] private RectTransform _bpmHitBar;
        Collider2D _bpmCol; Image _bpmHitImg;

        [Space]
        [SerializeField]
        private Color _colorOk, _colorExpire;
        float _greenBlue;
        [SerializeField] Image _cprPulsesHelper;
        [SerializeField] Image _bpmSliderHelper;

        [SerializeField] private AnimationCurve _sliderTimeCurve;
        public float BpmTimer = 0;
        readonly Collider2D[] _colliders = new Collider2D[1];

        float _bpmCounter;
        float _lastHitTime;

        public int TargetBPM;
        public float HitEfficiency;
        [SerializeField] float _bpmMultiplier;
        public int CprPushLimit;
        const float _delayInput = .2f;
        int _cprPushCount;
        float _pulseTimer;
        public UnityEvent hitEvent = new();
        [SerializeField] GameObject _cprSoundSourceObject;
        AudioSource _cprSoundPlayer;
        [SerializeField] AudioClip _cprSound;
        [SerializeField] UnityEvent<bool> _expireEvent;
        bool _missed; bool _invokedExpire = false;
        
        void Start()
        {
            try
            {
                if (LevelSetup.Instance.HandsOnly)
                {
                    CprPushLimit = 100;
                    _cprSoundPlayer = _cprSoundSourceObject.AddComponent<AudioSource>();
                    hitEvent.AddListener(() => _cprSoundPlayer.PlayOneShot(_cprSound));
                }
                else { CprPushLimit = 30; }
                if (LevelSetup.Instance.MusicChange)
                {
                    TargetBPM = 103;
                    _bpmMultiplier = 0.858333f; // 103 / 120
                }
                else
                {
                    TargetBPM = 120;
                    _bpmMultiplier = 1f;
                }
                 
            }
            catch
            {
                Debug.LogWarning("no LevelSetup singleton, using old default BPMUI values");
                CprPushLimit = 30;
                TargetBPM = 120;
                _bpmMultiplier = 1;
            }
            _bpmCol = _bpmHitBar.GetComponent<Collider2D>();
            _bpmHitImg = _bpmHitBar.GetComponent<Image>();
            _counterTransform = _bpmUICounter.GetComponent<RectTransform>();
            _cprPushCount = 0;
            enabled = !disableAfterStart;
        }
        void Update()
        {
            UpdateTimerBar();
        }
        void FixedUpdate()
        {
            BpmTimer += Time.fixedDeltaTime * _bpmMultiplier;
            BpmTimer = Mathf.Repeat(BpmTimer, 1f);
            _lastHitTime += Time.fixedDeltaTime;
        }
        void UpdateTimerBar()
        {
            _bpmHelperBar.value = _sliderTimeCurve.Evaluate(BpmTimer);
        }
        public void CPRHit()
        {
            if (_lastHitTime < _delayInput || _pushDepthBar.value == 0) return;
            _bpmCounter = 60 / _lastHitTime;
            _bpmCounter = Mathf.Round(_bpmCounter);
            if (_lastHitTime > .65f || _lastHitTime < .4f) _missed = true;
            _lastHitTime = 0;

            _cprPushCount++;
            _cprPushCounter.text = _cprPushCount.ToString();
            if(_cprPushCount < CprPushLimit)
            {
                if (_bpmCol.GetContacts(_colliders) > 0)
                {
                    if (!_missed)
                    {
                        _bpmCounter = TargetBPM;
                        GetEfficiency(_bpmCounter, TargetBPM);
                        StartCoroutine(IPulseBPMCounter(.2f));
                    }
                    else
                    {
                        _greenBlue = GetEfficiency(_bpmCounter, TargetBPM);
                        _bpmUICounter.color = new Color(1f, _greenBlue, _greenBlue);
                    }
                    StartCoroutine(IPulseHitBar(.2f));
                }
                else
                {
                    _greenBlue = GetEfficiency(_bpmCounter, TargetBPM);
                    _bpmUICounter.color = new Color(1f, _greenBlue, _greenBlue);
                    if(_bpmCounter == TargetBPM) StartCoroutine(IPulseBPMCounter(.2f));
                }
            }
            else
            {
                _bpmUICounter.color = _colorExpire;
                _bpmHitImg.color = _colorExpire;
                _cprPushCounter.color = _colorExpire;
                _cprPulsesHelper.enabled = false;
                _bpmSliderHelper.enabled = false;
                if (!_invokedExpire)_expireEvent.Invoke(true);
                _invokedExpire = true;
            }

            _bpmUICounter.text = _bpmCounter.ToString("0");
            _missed = false;
        }
        public float GetBPM()
        {
            if (_bpmCounter > 130) return 0;
            return _bpmCounter;
        }
        public float GetEfficiency(float counter, float target)
        {
            HitEfficiency = Mathf.PingPong(counter, target) / target;
            return HitEfficiency;
        }
        public float GetEfficiency()
        {
            //Debug.Log("GetEfficiency Called returning: " + HitEfficiency);
            return HitEfficiency;
        }
        IEnumerator IPulseHitBar(float duration)
        {
            hitEvent.Invoke();

            for (_pulseTimer = 0; _pulseTimer <= duration; _pulseTimer += Time.deltaTime)
            {
                _bpmHitBar.localScale = Vector3.one + .5f * _sliderTimeCurve.Evaluate(_pulseTimer / duration) * Vector3.one;
                yield return null;
            }
            _bpmHitBar.localScale = Vector3.one;
        }
        IEnumerator IPulseBPMCounter(float duration)
        {
            _bpmUICounter.color = _colorOk;
            for (_pulseTimer = 0; _pulseTimer <= duration;)
            {
                _counterTransform.localScale = Vector3.one + .5f * _sliderTimeCurve.Evaluate(_pulseTimer / duration) * Vector3.one;
                yield return null;
            }
            _counterTransform.localScale = Vector3.one;
        }
        public void ResetUI()
        {
            if (_bpmHitImg == null) return; // bandage. technical debt
            _bpmCounter = 0;
            _lastHitTime = 0;
            _cprPushCount = 0;
            _expireEvent.Invoke(false);
            _invokedExpire = false;

            _bpmUICounter.color = Color.white;
            _bpmHitImg.color = _colorOk;
            _cprPushCounter.color = Color.white;
            _cprPushCounter.text = "0";
        }
        public Slider GetBpmSlider() => _bpmHelperBar;
    }
}