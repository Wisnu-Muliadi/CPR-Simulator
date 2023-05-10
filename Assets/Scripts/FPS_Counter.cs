using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS_Counter : MonoBehaviour
{
    public TextMeshProUGUI timeText, fpsText;
    [SerializeField] private float deltaTime, elapsed;
    string[] _startString;
    public float updateTime;
    void Start()
    {
        _startString = new[] { timeText.text, fpsText.text };
    }
    void Update()
    {
        elapsed += Time.deltaTime;
        if (updateTime < elapsed)
        {
            deltaTime = Time.deltaTime;
            timeText.text = _startString[0] + deltaTime.ToString();

            fpsText.text = _startString[1] + (1f / deltaTime).ToString("0");
            elapsed = 0;
        }
    }
}
