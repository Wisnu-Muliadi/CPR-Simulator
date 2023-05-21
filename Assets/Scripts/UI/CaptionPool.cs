using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInterface
{
    public class CaptionPool : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _captionPool;
        [SerializeField] private float _showDuration = 3;

        private readonly List<CaptionText> _captionText = new();
        private readonly List<string> _captionStrings = new();

        private int _poolLastIndex = 0;
        void Start()
        {
            for (int i = 0; i < _captionPool.Count; i++)
            {
                _captionText.Add(_captionPool[i].GetComponent<CaptionText>());
                _poolLastIndex++;
            }
            _poolLastIndex -= 1;
        }
        public void EnqueueCaption(string captionText, float showDuration)
        {
            if(_captionStrings.Count < 6)
                _captionStrings.Add(captionText);
            StopAllCoroutines();
            StartCoroutine(IShowCaption(showDuration));
        }
        public void EnqueueCaption(string captionText)
        {
            if(_captionStrings.Count < 6)
                _captionStrings.Add(captionText);
            StopAllCoroutines();
            StartCoroutine(IShowCaption(_showDuration));
        }
        private IEnumerator IShowCaption(float showDuration)
        {
            while(_captionStrings.Count > 0)
            {
                yield return new WaitUntil(() => _captionPool[_poolLastIndex].activeSelf == false);
                for (int i = 0; i < _captionPool.Count; i++)
                {
                    if (_captionPool[i].activeSelf) continue;

                    _captionPool[i].SetActive(true);
                    _captionText[i].TextDuration = showDuration;
                    _captionText[i].Text.text = _captionStrings[0];
                    break;
                }
                _captionStrings.RemoveAt(0);
            }
        }
    }
}