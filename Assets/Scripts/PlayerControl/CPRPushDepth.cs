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
        [SerializeField] GameObject[] _objectsToHide;
        [SerializeField] Image[] _images;
        [SerializeField] TextMeshProUGUI[] _texts;
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
    }
}