using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public interface IMenuInteractable
    {
        void Interact();
        void ShowText();
    }

    public class MenuMouseRay : MonoBehaviour
    {
        [SerializeField] Camera thisCam;
        [SerializeField] LayerMask _layerMask;
        Ray _mouseRay;
        void Update()
        {
            _mouseRay = thisCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_mouseRay, out RaycastHit hitInfo, 100, _layerMask))
            {
                IMenuInteractable menuInteractable = hitInfo.transform.GetComponent<IMenuInteractable>();
                if (menuInteractable != null)
                {
                    menuInteractable.ShowText();
                    if (Input.GetMouseButtonDown(0))
                        menuInteractable.Interact();
                }
            }
        }
    }
}