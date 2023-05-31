using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInterface
{
    public class CallScriptCaptioner : MonoBehaviour
    {
        [SerializeField] CallScript _callScript;
        [SerializeField] CaptionPool _captionPool;
        public void InsertCallScriptToPool()
        {
            _captionPool.CallScriptParser(_callScript);
        }
    }
}