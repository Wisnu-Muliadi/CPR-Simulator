using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace PhoneCallSystem
{
    public class PhoneDisableManager : MonoBehaviour
    {
        public static PhoneDisableManager instance;

        [SerializeField] private FirstPersonController player = null;

        void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }
        }

        public void DisablePlayer(bool disable)
        {
            if (disable)
            {
                player.enabled = false;
                PhoneUIManager.instance.ShowCrosshair(false);
            }

            if (!disable)
            {
                player.enabled = true;
                PhoneUIManager.instance.ShowCrosshair(true);
            }
        }
    }
}
