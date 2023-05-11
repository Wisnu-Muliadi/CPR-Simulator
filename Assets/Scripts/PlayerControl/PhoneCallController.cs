using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace PlayerControl
{
    public class PhoneCallController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _phoneInputField;
        [SerializeField] private Animator _phoneAnimator;
        [SerializeField] private UnityEvent _enterCallEvent = new();
        private string _numberField = "";
        private bool _phoneActive = false;
        void Awake()
        {
            if (_phoneAnimator == null) _phoneAnimator = GetComponent<Animator>();
        }
        void Update()
        {
            if (Input.GetButtonDown("Phone"))
            {
                UsePhone();
            }
        }
        private void UsePhone()
        {
            if (!_phoneActive)
            {
                _numberField = "";
                _phoneInputField.text = "";
                _phoneActive = true;
                _phoneAnimator.Play("Start");
                _phoneAnimator.SetFloat("Speed", 1);
                _phoneAnimator.Play("PhoneSlideIn", 1, 0);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                GlobalInstance.Instance.UIManager.MouseDisplayAdd(true);
            }
            else
            {
                _phoneActive = false;
                _phoneAnimator.Play("PhoneSlideIn", 1, 1);
                _phoneAnimator.SetFloat("Speed", -1);
                Cursor.lockState = CursorLockMode.Locked;
                GlobalInstance.Instance.UIManager.MouseDisplayAdd(false);
            }
        }
        private void UpdateField() => _phoneInputField.text = _numberField;
        public void AddNumber(string number)
        {
            _numberField += number;
            UpdateField();
        }
        public void Backspace()
        {
            if (_numberField == "") return;
            _numberField = _numberField.Remove(_numberField.Length - 1, 1);
            UpdateField();
        }
        public void MakeCall()
        {
            if (_phoneInputField.text == "112")
            {
                _phoneAnimator.Play("Calling");
                _enterCallEvent.Invoke();
                StartCoroutine(nameof(ICallSequence));
            }
            else
                UsePhone();
        }

        private IEnumerator ICallSequence()
        {
            enabled = false;
            yield return new WaitForSecondsRealtime(1.6f);
            _phoneAnimator.Play("PhoneSlideDown", 1, .5f);
            GlobalInstance.Instance.UIManager.MouseDisplayAdd(false);
        }
    }
}