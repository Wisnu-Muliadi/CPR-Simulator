using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UserInterface
{
    public class CaptionPool : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _captionPool; // Use Queue instead. For future works.
        [SerializeField] private float _showDuration = 3;
        Animation _capAnimation;
        [System.Serializable]
        struct Caption
        {
            public Color captionColor;
            public string caption;
            public float duration;
        }
        readonly List<Caption> _caption = new();
        public static int PoolOccupant = 0;
        private int _poolMaxOccupant = 0;
        private readonly List<CaptionText> _captionText = new();
        private UnityEvent _endCaptionEvent = new();

        private WaitForSeconds waitDuration;
        private bool _routineStarted = false;
        void Awake()
        {
            _capAnimation = GetComponent<Animation>();
        }
        void Start()
        {
            for (int i = 0; i < _captionPool.Count; i++)
            {
                _captionText.Add(_captionPool[i].GetComponent<CaptionText>());
                _poolMaxOccupant++;
            }
        }
        void StartQueueRoutine()
        {
            if (_routineStarted) return;
            StartCoroutine(IQueueCaption());
        }
        public void EnqueueCaption(string captionText, float showDuration)
        {
            AddCaption(captionText, showDuration);
            StartQueueRoutine();
        }
        public void AddCaption(string captionText, float showDuration)
        {
            Caption caption = new();
            caption.captionColor = Color.white;
            caption.caption = captionText;
            caption.duration = showDuration;
            _caption.Add(caption);
        }
        public void CallScriptParser(CallScript callScript, UnityEvent postEvent)
        {
            _caption.RemoveAll(x=>true);
            for(int i = 0; i < callScript.Captions.Count; i++)
            {
                Caption cap = new();
                cap.captionColor = callScript.Captions[i].CaptionColor;
                cap.duration = callScript.Captions[i].duration;
                for (int j = 0; j < callScript.Captions[i].Sentence.Length; j++)
                {
                    cap.caption = callScript.Captions[i].Sentence[j];
                    _caption.Add(cap);
                }
            }
            _endCaptionEvent = postEvent;
            StartQueueRoutine();
        }
        private IEnumerator IQueueCaption()
        {
            float waitTime = 0;
            bool initiation = true;
            _routineStarted = true;
            _capAnimation.Play("Caption Pulse");
            do
            {
                while (_caption.Count > 0)
                {
                    if (initiation)
                        initiation = false;
                    else
                    {
                        waitDuration = new(waitTime);
                        yield return waitDuration;
                    }

                    yield return new WaitUntil(() => PoolOccupant < _poolMaxOccupant); // Pause until the pool has available
                    for (int i = 0; i < _captionPool.Count; i++)
                    {
                        if (_captionPool[i].activeSelf) continue;

                        _captionText[i].Text.text = _caption[0].caption;
                        _captionText[i].Text.color = _caption[0].captionColor;
                        _captionText[i].TextDuration = _caption[0].duration + .5f;
                        _captionPool[i].SetActive(true);
                        break;
                    }
                    waitTime = _caption[0].duration;
                    _caption.RemoveAt(0);
                }
                _endCaptionEvent.Invoke();
                initiation = true;
                yield return new WaitUntil(()=> _captionPool.TrueForAll(x => !x.activeSelf));
            } while (_caption.Count > 0);
            _endCaptionEvent = new();
            _capAnimation.PlayQueued("Caption Hidden");
            _routineStarted = false;
        }
    }
}