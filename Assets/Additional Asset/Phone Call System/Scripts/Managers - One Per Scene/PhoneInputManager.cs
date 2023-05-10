using UnityEngine;

namespace PhoneCallSystem
{
    public class PhoneInputManager : MonoBehaviour
    {
        [Header("Raycast Pickup Input")]
        public KeyCode interactKey;

        [Header("Trigger Inputs")]
        public KeyCode triggerInteractKey;

        public static PhoneInputManager instance;

        /// <summary>
        /// INPUTS INSIDE: PhoneController & PhoneTrigger Scripts
        /// </summary>

        private void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }
        }
    }
}
