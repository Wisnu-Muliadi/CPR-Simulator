using UnityEngine;

namespace PhoneCallSystem
{
    public class PhoneTrigger : MonoBehaviour
    {
        [Header("Phone Model")]
        [SerializeField] private PhoneItem phoneModelObject = null;

        [Header("Player Tag")]
        [SerializeField] private const string playerTag = "Player";

        private bool canUse;

        private void Update()
        {
            ShowKeypadInput();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                canUse = true;
                PhoneUIManager.instance.ShowInteractPrompt(canUse);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                canUse = false;
                PhoneUIManager.instance.ShowInteractPrompt(canUse);
            }
        }

        private void ShowKeypadInput()
        {
            if (canUse)
            {
                if (Input.GetKeyDown(PhoneInputManager.instance.triggerInteractKey))
                {
                    PhoneUIManager.instance.ShowInteractPrompt(false);
                    phoneModelObject.ShowPhoneUI();
                }
            }
        }
    }
}
