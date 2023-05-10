using UnityEngine;

namespace PhoneCallSystem
{
    public class PhoneItem : MonoBehaviour
    {
        [SerializeField] private PhoneController _phonepadController = null;

        public void ShowPhoneUI()
        {
            _phonepadController.ShowKeypad();
        }
    }
}
