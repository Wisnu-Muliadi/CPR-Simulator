using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerControl
{
    public class CPRPushDepth : MonoBehaviour
    {
        private Slider _slider;
        [SerializeField] GameObject[] _objectsToHide;
        void Start()
        {
            _slider = GetComponent<Slider>();
        }
        public void Check()
        {
            switch (_slider.value)
            {
                case 0:
                    foreach(GameObject gObject in _objectsToHide)
                    {
                        gObject.SetActive(false);
                    }
                    break;
                default:
                    foreach (GameObject gObject in _objectsToHide)
                    {
                        gObject.SetActive(true);
                    }
                    break;
            }
        }
    }
}