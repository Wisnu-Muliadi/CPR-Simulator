using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

namespace UserInterface
{
    public class DisableOnNonMobileDevice : MonoBehaviour
    {
        bool _isMobile = false;
        [SerializeField] bool _startDisabled = false;
        void Start()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            _isMobile = IsMobile();
#endif
            if (!_isMobile)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(!_startDisabled);
            }
        }
        public void EnableGameObject(bool enable)
        {
            if (_isMobile)
            {
                gameObject.SetActive(enable);
            }
        }
#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern bool IsMobile();
#endif
    }
}