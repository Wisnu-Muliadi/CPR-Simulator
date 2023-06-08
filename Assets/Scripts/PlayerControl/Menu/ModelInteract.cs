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
        [SerializeField] GameObject _toEnable;
        [SerializeField] GameObject _toDisable;

        [SerializeField] EnterLevelScene _enterLevelScript;
        [SerializeField] int _changeTargetScene;
        bool showText;
        public void Interact()
        {
            showText = false;
            if(_textObject != null)
                _textObject.enabled = showText;
            _enterLevelScript.TargetGameSceneIndex = _changeTargetScene;
            _toEnable.SetActive(true);
            _animator.SetTrigger("Interact");
            _toDisable.SetActive(false);
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