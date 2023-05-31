using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PlayerControl
{
    public class ModelInteract : MonoBehaviour, IMenuInteractable
    {
        [SerializeField] TextMeshPro _textObject;
        [SerializeField] Animator _animator;
        bool showText;
        public void Interact()
        {
            _animator.SetTrigger("Dance");
        }

        public void ShowText()
        {
            showText = true;
        }
        void FixedUpdate()
        {
            _textObject.enabled = showText;
            showText = false;
        }
    }
}