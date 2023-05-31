using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayerControl;

namespace UserInterface
{
    public class UITarget : MonoBehaviour
    {
        public SkinnedMeshRenderer patientRenderer;

        RectTransform _rectTransform;
        Image _image;
        TextMeshProUGUI _text;
        TrackingUI _trackingUI;
        ICPRable cprAble;
        
        void OnEnable()
        {
            _rectTransform = InteractTargetManager.Instance.GetTargetTransform();
            if (_rectTransform != null)
                _rectTransform.gameObject.SetActive(true);
            else
                enabled = false;
            
            if(_text == null)
                _text = _rectTransform.GetComponentInChildren<TextMeshProUGUI>();

            if (transform.parent.TryGetComponent(out cprAble))
            {
                _text.text = cprAble.GetDescription();
            }
            else if (TryGetComponent(out cprAble))
            {
                _text.text = cprAble.GetDescription();
            }
            if(_trackingUI != null && cprAble != null)
                _trackingUI.AssignCPRAble(cprAble);
        }
        void Start()
        {
            if (_rectTransform != null)
            {
                _image = _rectTransform.GetComponent<Image>();
                _trackingUI = _rectTransform.GetComponent<TrackingUI>();
                _trackingUI.AssignCPRAble(cprAble);
            }
        }
        void Update()
        {
            if (_rectTransform != null)
            {
                _rectTransform.position = GlobalInstance.Instance.mainCam.WorldToScreenPoint(transform.position);
            }
            else
                enabled = false;
        }
        private void FixedUpdate()
        {
            if (patientRenderer.isVisible)
                _image.enabled = true;
            else
                _image.enabled = false;
        }
        void OnDisable()
        {
            if (_rectTransform != null)
                _rectTransform.gameObject.SetActive(false);
        }
        public void Disallow()
        {
            _image.CrossFadeAlpha(.5f, .2f, true);
            _trackingUI.Disallow();
        }
        public void Allow()
        {
            _image.CrossFadeAlpha(1f, .2f, true);
            _trackingUI.Allow();
        }
    }
}