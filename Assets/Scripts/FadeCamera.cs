using UnityEngine;
using UnityEngine.Events;

public class FadeCamera : MonoBehaviour
{
    public AnimationCurve FadeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.6f, 0.7f, -1.8f, -1.2f), new Keyframe(1, 0));
    [SerializeField] UnityEvent _doneEvent;

    public Color fadeColor = new(1, 1, 1, 0);
    private float _alpha = 1;
    private Texture2D _texture;
    [SerializeField]
    private bool _begin, _done;
    private float _time;

    public void Reset()
    {
        _done = false;
        _alpha = 1;
        _time = 0;
    }

    [RuntimeInitializeOnLoadMethod]
    public void RedoFade()
    {
        Reset();
    }
    public void StartFade() => _begin = true;
    public void OnGUI()
    {
        if (_done)
        {
            enabled = false;
            _doneEvent.Invoke();
            return;
        }
        if (_texture == null) _texture = new Texture2D(1, 1);

        fadeColor.a = _alpha;
        _texture.SetPixel(0, 0, fadeColor);
        _texture.Apply();

        if (!_begin)
            _time = 0;
        else
            _time += Time.deltaTime;

        _alpha = FadeCurve.Evaluate(_time);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);

        if (_alpha <= 0) _done = true;
    }
}