using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using TMPro;

namespace PlayerControl
{
    public class GenericInteraction : MonoBehaviour, IMenuInteractable
    {
        [SerializeField] CinemachineVirtualCamera _virtualCamToActivate;
        [SerializeField] TextMeshPro _textMeshPro;
        [SerializeField] UnityEvent _interactedEvent, _finishInteractionEvent;
        bool _showText = false;

        public void Interact()
        {
            _virtualCamToActivate.enabled = true;
            _interactedEvent.Invoke();
            _textMeshPro.enabled = false;
        }
        public void BackOffInteraction()
        {
            _virtualCamToActivate.enabled = false;
            _finishInteractionEvent.Invoke();
        }
        public void ShowText()
        {
            _showText = true;
        }
        void FixedUpdate()
        {
            _textMeshPro.enabled = _showText;
            _showText = false;
        }
    }
}