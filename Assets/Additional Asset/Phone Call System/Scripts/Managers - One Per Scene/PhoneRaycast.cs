using UnityEngine;

namespace PhoneCallSystem
{
    public class PhoneRaycast : MonoBehaviour
    {
        [Header("Raycast Features")]
        [SerializeField] private float interactDistance = 5;
        private PhoneItem raycastedObj;
        private Camera _camera;

        private const string raycastTag = "Phone";

        void Start()
        {
            _camera = GetComponent<Camera>();
        }

        void Update()
        {
            if (Physics.Raycast(_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f)), transform.forward, out RaycastHit hit, interactDistance))
            {
                var selectedItem = hit.collider.GetComponent<PhoneItem>();
                if (selectedItem != null && selectedItem.CompareTag(raycastTag))
                {
                    raycastedObj = selectedItem;
                    HighlightCrosshair(true);
                }
                else
                {
                    ClearExaminable();
                }
            }
            else
            {
                ClearExaminable();
            }

            if (raycastedObj != null)
            {
                if (Input.GetKeyDown(PhoneInputManager.instance.interactKey))
                {
                    raycastedObj.ShowPhoneUI();
                }
            }
        }

        private void ClearExaminable()
        {
            if (raycastedObj != null)
            {
                HighlightCrosshair(false);
                raycastedObj = null;
            }
        }

        void HighlightCrosshair(bool on)
        {
            PhoneUIManager.instance.HighlightCrosshair(on);
        }
    }
}
