using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PhoneCallSystem
{
    [System.Serializable]
    public class PhoneCodes
    {
        public string phoneCode;
        public Sound phoneClip;
    }

    public class PhoneController : MonoBehaviour
    {
        [Header("Phone UI Type")]
        [SerializeField] private phoneType _phoneType = phoneType.None;
        private enum phoneType { None, Pay, Office, Mobile };

        [Header("Keypad Parameters")]
        public int characterLim;

        [Header("Phone Codes")]
        [SerializeField] private PhoneCodes[] phoneCodesList = null;

        [Header("Sound Effects")]
        [SerializeField] private Sound _deadDialSound = null;
        [SerializeField] private Sound _singleBeepSound = null;

        [Header("Trigger Type - ONLY if using a trigger event")]
        [SerializeField] private PhoneTrigger triggerObject = null;
        [SerializeField] private bool isPhoneTrigger = false;

        private AudioSource mainAudio;

        void Awake()
        {
            mainAudio = GetComponent<AudioSource>();
        }

        public void ShowKeypad()
        {
            PhoneDisableManager.instance.DisablePlayer(true);
            PhoneUIManager.instance.SetPhoneController(this);
            SwitchPhoneType(true);

            if (isPhoneTrigger)
            {
                PhoneUIManager.instance.ShowInteractPrompt(false);
                triggerObject.enabled = false;
            }
        }

        public void CloseKeypad()
        {
            SwitchPhoneType(false);

            if (isPhoneTrigger)
            {
                PhoneUIManager.instance.ShowInteractPrompt(true);
                triggerObject.enabled = true;
            }
        }

        void SwitchPhoneType(bool on)
        {
            switch (_phoneType)
            {
                case phoneType.Pay: PhoneUIManager.instance.ShowPayPhoneCanvas(on);
                    break;
                case phoneType.Office: PhoneUIManager.instance.ShowOfficePhoneCanvas(on);
                    break;
                case phoneType.Mobile: PhoneUIManager.instance.ShowMobilePhoneCanvas(on);
                    break;
            }
        }

        public void CheckCode(InputField numberInputField)
        {
            try
            {
                StopAudio();
                var code = phoneCodesList.First(x => x.phoneCode == numberInputField.text);
                PhoneAudioManager.instance.Play(code.phoneClip.name);
                //mainAudio.PlayOneShot(code.phoneClip, 1f);
            }
            catch
            {
                StopAudio();
                DeadDialSound();
            }
        }

        public void SingleBeepSound()
        {
            PhoneAudioManager.instance.Play(_singleBeepSound.name);
        }

        void DeadDialSound()
        {
            PhoneAudioManager.instance.Play(_deadDialSound.name);
        }

        public void StopAudio()
        {
            PhoneAudioManager.instance.StopAll();
            StopDeadDialSound();
        }

        void StopDeadDialSound()
        {
            PhoneAudioManager.instance.StopPlaying(_deadDialSound.name);
        }
    }
}
