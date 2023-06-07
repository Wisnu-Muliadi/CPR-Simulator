using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CardiacPatient;

namespace PlayerControl
{
    public interface IInteractable
    {
        UnityAction InteractAction { get; set; }
        void Interact(PlayerInteraction player);

        string GetDescription();
    }
    /// <summary>
    /// Decides when to Switch to CPR Interaction and FPS Control
    /// Attach this to FPS PlayerController
    /// </summary>
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] Camera _cam;
        [SerializeField] float _rayDistance = 3;
        [SerializeField] LayerMask _layerMask;

        Ray ray;
        RaycastHit hitinfo;

        public UnityEvent<string> eventUI_Update;
        public UnityEvent<bool> eventUI_Enable;
        // Start is called before the first frame update
        void Start()
        {
            if (_cam == null)
                _cam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            HandleInteractable();
        }

        private void HandleInteractable()
        {
            ray = _cam.ViewportPointToRay(new Vector3(.5f, .5f));
            if (Physics.Raycast(ray, out hitinfo, _rayDistance, _layerMask))
            {
                IInteractable interactable = hitinfo.collider.GetComponent<IInteractable>();
                if (interactable == null)
                {
                    //Debug.DrawLine(ray.origin, ray.GetPoint(_rayDistance), Color.blue);
                    eventUI_Enable.Invoke(false);
                }
                else
                {
                    //Debug.DrawLine(ray.origin, ray.GetPoint(_rayDistance), Color.green);
                    eventUI_Enable.Invoke(true);
                    eventUI_Update.Invoke(interactable.GetDescription());
                    if (Input.GetButtonDown("Interact"))
                    {
                        interactable.Interact(this);
                        eventUI_Enable.Invoke(false);
                    }
                }
            }
            else
            {
                //Debug.DrawLine(ray.origin, ray.GetPoint(_rayDistance), Color.white);
                eventUI_Enable.Invoke(false);
            }
        }
    }
}