using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class CPRCamGiveBreathAnimation : MonoBehaviour
    {
        [SerializeField] List<AnimationClip> _animationClips;
        [SerializeField] float _delayForEachGive = 3f;
        private float _timer = 0;
        private bool _pullingBreath = false;
        Animation _animation;
        
        private bool _hover;
        private UserInterface.GiveBreathUILogic _giveBreathUI;
        void Start()
        {
            _hover = false;
            _animation = gameObject.AddComponent<Animation>();
            for (int i = 0; i < _animationClips.Count; i++)
                _animation.AddClip(_animationClips[i], i.ToString());
        }
        void OnEnable()
        {
            _timer = _delayForEachGive;
            try
            {
                _giveBreathUI = GlobalInstance.Instance.UIManager.giveBreathUI;
                _giveBreathUI.PointerEnterEvent.AddListener(SwitchHover);
            }
            catch
            {
                
            }
        }
        void OnDisable()
        {
            _hover = false;
            if (_giveBreathUI != null)
            {
                _giveBreathUI.PointerEnterEvent.RemoveListener(SwitchHover);
            }
        }
        void Update()
        {
            UpdateTimer();
            if (_timer < _delayForEachGive) return;
            if (Input.GetMouseButtonDown(0) && _hover)
            {
                PullBreath();
                _pullingBreath = true;
            }
            else if (Input.GetMouseButtonUp(0) && _pullingBreath)
            {
                if (!_hover)
                {
                    _animation.Play("0");
                    return;
                }
                GaveBreath();
                _timer = 0;
                _pullingBreath = false;
            }
        }
        public void SwitchHover(bool boolean)
        {
            _hover = boolean;
        }
        private void PullBreath()
        {
            _animation.Play("1");
        }
        private void GaveBreath()
        {
            _animation.Play("2");
            _animation.CrossFadeQueued("0", .2f, QueueMode.CompleteOthers);
        }
        private void UpdateTimer()
        {
            if (_timer < 10)
            {
                _timer += Time.deltaTime;
            }
        }
    }
}