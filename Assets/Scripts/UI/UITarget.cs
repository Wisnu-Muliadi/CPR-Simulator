using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInterface
{
    public class UITarget : MonoBehaviour
    {
        RectTransform _rectTransform;
        void OnEnable()
        {
            _rectTransform = InteractTargetManager.Instance.GetTargetTransform();
            if (_rectTransform != null)
                _rectTransform.gameObject.SetActive(true);
            else
                enabled = false;
        }
        void Update()
        {
            if (_rectTransform != null)
                _rectTransform.position = GlobalInstance.Instance.mainCam.WorldToScreenPoint(transform.position);
            else
                enabled = false;
        }
        void OnDisable()
        {
            if (_rectTransform != null)
                _rectTransform.gameObject.SetActive(false);
        }
    }
}