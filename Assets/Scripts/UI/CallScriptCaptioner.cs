using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UserInterface
{
    public class CallScriptCaptioner : MonoBehaviour
    {
        [SerializeField] CaptionPool _captionPool;
        
        [SerializeField] CallScript _callScript;
        [SerializeField] UnityEvent _postCaptionEvent;
        [SerializeField] CallScript[] _callScripts;
        [SerializeField, Tooltip("Sesuaikan Dengan Array Call Scripts")] UnityEvent[] _postCaptionEvents;

        void Start()
        {
            if(_captionPool == null) _captionPool = GlobalInstance.Instance.UIManager.captionPool;
        }
        public void InsertCallScriptToPool()
        {
            _captionPool.CallScriptParser(_callScript, _postCaptionEvent);
        }
        public void InsertCallScriptToPool(int index)
        {
            _captionPool.CallScriptParser(_callScripts[index], _postCaptionEvents[index]);
        }
    }
}