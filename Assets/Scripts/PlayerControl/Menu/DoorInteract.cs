using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

namespace PlayerControl
{
    public class DoorInteract : MonoBehaviour, IMenuInteractable
    {
        [SerializeField] TextMeshPro _textObject;
        [SerializeField] PlayableDirector _director;
        [SerializeField] MenuMouseRay _menuRayControls;
        bool showText;
        public void Interact()
        {
            showText = false;
            _menuRayControls.enabled = false;
            _director.Play();
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

        void OpenDoorSound()
        {
            GetComponent<AudioSource>().Play();
        }
    }
}