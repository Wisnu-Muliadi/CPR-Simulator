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
        Image _image;
        RectTransform _rectTransform;
        TextMeshProUGUI _text;
        
        void OnEnable()
        {
            _rectTransform = InteractTargetManager.Instance.GetTargetTransform();
            if (_rectTransform != null)
                _rectTransform.gameObject.SetActive(true);
            else
                enabled = false;
            
            if(_text == null)
                _text = _rectTransform.GetComponentInChildren<TextMeshProUGUI>();

            if (transform.parent.TryGetComponent(out ICPRable cprAble))
            {
                _text.text = cprAble.GetDescription();
            }
            else if (TryGetComponent(out cprAble))
            {
                _text.text = cprAble.GetDescription();
            }
        }
        void Start()
        {
            _image = _rectTransform.GetComponent<Image>();
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
    }
}