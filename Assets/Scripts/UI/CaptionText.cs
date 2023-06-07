using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UserInterface
{
    public class CaptionText : MonoBehaviour
    {
        public TextMeshProUGUI Text { get; private set; }
        public float TextDuration = 3f;
        private Animation _animation;

        WaitForSecondsRealtime waitDuration = new(0);
        void Awake()
        {
            Text = GetComponent<TextMeshProUGUI>();
            _animation = GetComponent<Animation>();
            gameObject.SetActive(false);
        }
        void OnEnable()
        {
            transform.SetAsLastSibling();
            StartCoroutine(IEnable());
        }
        IEnumerator IEnable()
        {
            CaptionPool.PoolOccupant++;
            waitDuration = new(TextDuration);
            _animation.Play("CaptionFadeIn");
            yield return waitDuration;
            _animation.Play("CaptionFadeOut");
            yield return null;
        }
        // called by animation event
        public void DisableText()
        {
            gameObject.SetActive(false);
            CaptionPool.PoolOccupant--;
        }
    }
}