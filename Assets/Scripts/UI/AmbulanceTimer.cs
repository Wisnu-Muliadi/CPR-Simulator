using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class AmbulanceTimer : MonoBehaviour
    {
        Slider _ambulanceSlider;
        [SerializeField, Tooltip("in Seconds")] float _waitDuration = 300;
        float _currTime = 0;
        void Awake()
        {
            _ambulanceSlider = GetComponent<Slider>();
            _ambulanceSlider.maxValue = _waitDuration;
        }
        void FixedUpdate()
        {
            _currTime = Mathf.MoveTowards(_currTime, _waitDuration, Time.deltaTime);
            _ambulanceSlider.value = _currTime;
        }
    }
}