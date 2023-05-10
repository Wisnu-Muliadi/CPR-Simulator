using UnityEngine;
using UnityEngine.UI;

namespace PhoneCallSystem
{
    public class PhoneUIManager : MonoBehaviour
    {
        [Header("Crosshair")]
        [SerializeField] private Image crosshair = null;

        [Header("UI Prompt")]
        [SerializeField] private GameObject interactPrompt = null;

        [Header("Phone Type Input Fields")]
        [SerializeField] private InputField payPhoneCodeText = null;
        [SerializeField] private InputField officePhoneCodeText = null;
        [SerializeField] private InputField mobilePhoneCodeText = null;

        [Header("Phone Type Canvas Fields")]
        [SerializeField] private GameObject payPhoneCanvas = null;
        [SerializeField] private GameObject officePhoneCanvas = null;
        [SerializeField] private GameObject mobilePhoneCanvas = null;

        private bool firstClick;
        private PhoneController _phoneController;
        private phoneType _phoneType;
        private enum phoneType { None, Pay, Office, Mobile };

        public static PhoneUIManager instance;

        private void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }
        }

        public void SetPhoneController(PhoneController _myController)
        {
            _phoneController = _myController;
        }

        public void ShowPayPhoneCanvas(bool on)
        {
            payPhoneCanvas.SetActive(on);
            _phoneType = phoneType.Pay;
        }

        public void ShowOfficePhoneCanvas(bool on)
        {
            officePhoneCanvas.SetActive(on);
            _phoneType = phoneType.Office;
        }

        public void ShowMobilePhoneCanvas(bool on)
        {
            mobilePhoneCanvas.SetActive(on);
            _phoneType = phoneType.Mobile;
        }

        public void KeyPressString(string keyString)
        {
            _phoneController.SingleBeepSound();

            if (!firstClick)
            {
                if (payPhoneCodeText != null)
                {
                    payPhoneCodeText.text = string.Empty;
                }
                if (officePhoneCodeText != null)
                {
                    officePhoneCodeText.text = string.Empty;
                }
                if (mobilePhoneCodeText != null)
                {
                    mobilePhoneCodeText.text = string.Empty;
                }
                firstClick = true;
            }

            switch (_phoneType)
            {
                case phoneType.Pay:
                    if (payPhoneCodeText.characterLimit <= (_phoneController.characterLim - 1))
                    {
                        payPhoneCodeText.characterLimit++;
                        payPhoneCodeText.text += keyString;
                    }
                    break;
                case phoneType.Office:
                    if (officePhoneCodeText.characterLimit <= (_phoneController.characterLim - 1))
                    {
                        officePhoneCodeText.characterLimit++;
                        officePhoneCodeText.text += keyString;
                    }
                    break;
                case phoneType.Mobile:
                    if (mobilePhoneCodeText.characterLimit <= (_phoneController.characterLim - 1))
                    {
                        mobilePhoneCodeText.characterLimit++;
                        mobilePhoneCodeText.text += keyString;
                    }
                    break;
            }
        }

        public void KeyPressCall()
        {
            _phoneController.SingleBeepSound();
            switch (_phoneType)
            {
                case phoneType.Pay: _phoneController.CheckCode(payPhoneCodeText);
                    break;
                case phoneType.Office: _phoneController.CheckCode(officePhoneCodeText);
                    break;
                case phoneType.Mobile: _phoneController.CheckCode(mobilePhoneCodeText);
                    break;
            }
        }

        public void KeyPressClr()
        {
            _phoneController.SingleBeepSound();
            _phoneController.StopAudio();

            switch (_phoneType)
            {
                case phoneType.Pay:
                    payPhoneCodeText.characterLimit = 0;
                    payPhoneCodeText.text = string.Empty;
                    break;
                case phoneType.Office:
                    officePhoneCodeText.characterLimit = 0;
                    officePhoneCodeText.text = string.Empty;
                    break;
                case phoneType.Mobile:
                    mobilePhoneCodeText.characterLimit = 0;
                    mobilePhoneCodeText.text = string.Empty;
                    break;
            }
        }

        public void KeyPressClose()
        {
            _phoneController.SingleBeepSound();
            _phoneController.StopAudio();
            PhoneDisableManager.instance.DisablePlayer(false);
            KeyPressClr();
            _phoneController.CloseKeypad();
        }

        public void ShowInteractPrompt(bool on)
        {
            interactPrompt.SetActive(on);
        }

        public void ShowCrosshair(bool on)
        {
            crosshair.enabled = on;
            if (on)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void HighlightCrosshair(bool on)
        {
            if (on)
            {
                crosshair.color = Color.red;
            }
            else
            {
                crosshair.color = Color.white;
            }
        }
    }
}
