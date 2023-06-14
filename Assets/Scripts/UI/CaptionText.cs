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
        public bool WaitForInteract = false;
        private bool _interacted = false;

        WaitForSecondsRealtime waitDuration = new(0);
        void Awake()
        {
            Text = GetComponent<TextMeshProUGUI>();
            _animation = GetComponent<Animation>();
            gameObject.SetActive(false);
        }
        void OnEnable()
        {
            _interacted = false;
            transform.SetAsLastSibling();
            StartCoroutine(IEnable(WaitForInteract));
        }
        IEnumerator IEnable(bool waitForInteract)
        {
            CaptionPool.PoolOccupant++;
            waitDuration = new(TextDuration);
            _animation.Play("CaptionFadeIn");
            if (!waitForInteract) yield return waitDuration;
            else yield return new WaitUntil(InteractText);
            _animation.Play("CaptionFadeOut");
            yield return null;
        }
        private bool InteractText() => _interacted;
        public void InteractWithText() => _interacted = true;

        // called by animation event
        public void DisableText()
        {
            gameObject.SetActive(false);
            CaptionPool.PoolOccupant--;
        }
    }
}