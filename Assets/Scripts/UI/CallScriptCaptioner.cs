using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UserInterface
{
    public class CallScriptCaptioner : MonoBehaviour
    {
        [SerializeField] CaptionPool _captionPool;
        [SerializeField] ClickableCaptionPool _clickableCaptionPool;
        [SerializeField] bool useClickablePool = false;
        
        [SerializeField] CallScript _callScript;
        [SerializeField] UnityEvent _postCaptionEvent;
        [SerializeField] CallScript[] _callScripts;
        [SerializeField, Tooltip("Sesuaikan Dengan Array Call Scripts")] UnityEvent[] _postCaptionEvents;

        void Awake()
        {
            switch(useClickablePool)
            {
                case true:
                    if (_clickableCaptionPool == null) _clickableCaptionPool = GlobalInstance.Instance.UIManager.clickableCaptionPool;
                    break;
                case false:
                    if (_captionPool == null) _captionPool = GlobalInstance.Instance.UIManager.captionPool;
                    break;
            }
        }
        public void InsertCallScriptToPool()
        {
            if (useClickablePool) _clickableCaptionPool.CallScriptParser(_callScript, _postCaptionEvent);
            else _captionPool.CallScriptParser(_callScript, _postCaptionEvent);
        }
        public void InsertCallScriptToPool(int index)
        {
            if(useClickablePool) _clickableCaptionPool.CallScriptParser(_callScripts[index], _postCaptionEvents[index]);
            else _captionPool.CallScriptParser(_callScripts[index], _postCaptionEvents[index]);
        }
    }
}