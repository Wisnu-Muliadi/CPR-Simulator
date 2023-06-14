using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UserInterface
{
    public class ClickableCaptionPool : MonoBehaviour
    {
        CaptionPool captionPool;
        [SerializeField] List<CaptionText> _captionTextPool;
        Queue<CaptionText> _captionQueue = new();
        CaptionText currentCaption;
        Animation _capAnimation;
        [SerializeField] KeyCode _interactKey;
        [SerializeField] GameObject _nextButtonIndicator;
        struct Caption
        {
            public Color captionColor;
            public float maxDuration;
            public string caption;
        }
        List<Caption> _caption = new();
        UnityEvent _endCaptionEvent = new();

        float timer;
        void Awake()
        {
            foreach (CaptionText captionText in _captionTextPool)
            {
                _captionQueue.Enqueue(captionText);
            }
            _capAnimation = GetComponent<Animation>();
            captionPool = GetComponent<CaptionPool>();
        }
        void OnEnable()
        {
            _capAnimation.Play("Caption Pulse");
        }
        void OnDisable()
        {
            _capAnimation.Play("Caption Hidden");
        }
        void Update()
        {
            if (_caption.Count == 0)
            { currentCaption = null; _endCaptionEvent.Invoke(); _nextButtonIndicator.SetActive(false); enabled = false;  return; }

            if (Input.GetKeyDown(_interactKey) || timer >= _caption[0].maxDuration)
            {
                _caption.RemoveAt(0);
                _captionQueue.Enqueue(currentCaption);
                currentCaption.InteractWithText();

                DequeueOldestCaption();
            }
        }
        void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
        }
        private void DequeueOldestCaption()
        {
            if (_caption.Count == 0) return;
            currentCaption = _captionQueue.Dequeue();
            currentCaption.WaitForInteract = true;
            currentCaption.Text.text = _caption[0].caption;
            currentCaption.Text.color = _caption[0].captionColor;
            currentCaption.gameObject.SetActive(true);
            _nextButtonIndicator.SetActive(true);
            enabled = true;
            timer = 0;
        }
        public void CallScriptParser(CallScript callScript, UnityEvent postEvent)
        {
            Clear();
            if (captionPool != null) captionPool.Clear();
            for (int i = 0; i < callScript.Captions.Count; i++)
            {
                Caption cap = new();
                cap.captionColor = callScript.Captions[i].CaptionColor;
                cap.maxDuration = callScript.Captions[i].duration;
                for (int j = 0; j < callScript.Captions[i].Sentence.Length; j++)
                {
                    cap.caption = callScript.Captions[i].Sentence[j];
                    _caption.Add(cap);
                }
            }
            _endCaptionEvent = postEvent;
            
            DequeueOldestCaption();
        }
        public void Clear()
        {
            _caption.RemoveAll(x => true);
            if (currentCaption != null)
            {
                _captionQueue.Enqueue(currentCaption);
                currentCaption.InteractWithText();
            }
        }
    }
}