using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CallScript", menuName = "ScriptableObjects/CreateNewCallScript")]
public class CallScript : ScriptableObject
{
    [System.Serializable]
    public struct CaptionStruct
    {
        public string[] Sentence;
        public Color CaptionColor;
        [Tooltip("Delay until next Text Appear")]public float duration;
    }
    public List<CaptionStruct> Captions;
}
