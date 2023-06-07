using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PlayerControl
{
    public class CPRPushDepth : MonoBehaviour
    {
        private Slider _slider;
        [SerializeField] TextMeshProUGUI _depthText;
        [SerializeField] GameObject[] _objectsToHide;
        [SerializeField] Image[] _images;
        [SerializeField] TextMeshProUGUI[] _texts;

        float _greenBlue = 0;
        void Awake()
        {
            _slider = GetComponent<Slider>();
        }
        void OnEnable()
        {
            Check();
        }
        public void Check()
        {
            switch (_slider.value)
            {
                case 0:
                    foreach (Image img in _images)
                    {
                        img.CrossFadeAlpha(.5f, .2f, true);
                    }
                    foreach (TextMeshProUGUI tmpro in _texts)
                    {
                        tmpro.CrossFadeAlpha(.5f, .2f, true);
                    }
                    break;
                default:
                    foreach (Image img in _images)
                    {
                        img.CrossFadeAlpha(1f, .2f, true);
                    }
                    foreach (TextMeshProUGUI tmpro in _texts)
                    {
                        tmpro.CrossFadeAlpha(1f, .2f, true);
                    }
                    break;
            }
        }
        public void UpdateDepth()
        {
            _depthText.text = _slider.value.ToString("0.0") + "cm";
            _greenBlue = .2f * Mathf.PingPong(_slider.value, 5);
            _depthText.color = new Color(1f, _greenBlue, _greenBlue);
        }
    }
}