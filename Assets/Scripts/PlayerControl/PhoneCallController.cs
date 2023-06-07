using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace PlayerControl
{
    public class PhoneCallController : MonoBehaviour
    {
        [SerializeField] bool _isTutorial = false;
        [SerializeField] private TMP_InputField _phoneInputField;
        [SerializeField] private Animator _phoneAnimator;
        public UnityEvent EnterCallEvent = new();
        [SerializeField, Tooltip("false when Using Phone")] private UnityEvent<bool> _notUsingPhone = new();
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
                _notUsingPhone.Invoke(false);
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
                _notUsingPhone.Invoke(true);
                _phoneActive = false;
                _phoneAnimator.Play("PhoneSlideIn", 1, 1);
                _phoneAnimator.SetFloat("Speed", -1);
                Cursor.lockState = CursorLockMode.Locked;
                GlobalInstance.Instance.UIManager.MouseDisplayAdd(false);
            }
        }
        public void PhoneSlideOut()
        {
            _phoneAnimator.Play("PhoneSlideIn", 1, 1);
            _phoneAnimator.SetFloat("Speed", -1);
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
                StartCoroutine(nameof(ICallSequence));
            }
            else
                UsePhone();
        }

        private IEnumerator ICallSequence()
        {
            enabled = false;
            _notUsingPhone.Invoke(true);
            yield return new WaitForSecondsRealtime(1.6f);
            if (_isTutorial)
            {
                UsePhone();
                yield return new WaitForSecondsRealtime(1.6f);
            }
            else
            {
                _phoneAnimator.Play("PhoneSlideDown", 2, .5f);
                GlobalInstance.Instance.UIManager.MouseDisplayAdd(false);
            }
            EnterCallEvent.Invoke();
        }
    }
}