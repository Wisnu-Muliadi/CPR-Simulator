using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerControl;

namespace UserInterface {
    public class InteractTargetManager : MonoBehaviour
    {
        public static InteractTargetManager Instance;

        [SerializeField] Sprite _iInteractableUI, _iCPRableUI;
        [SerializeField] List<RectTransform> _uiTargetsPool;

        [System.Serializable]
        class TargetClass
        {
            public GameObject TargetObject;
            public UITarget UITarget;
            public void AssignUITarget() => UITarget = TargetObject.AddComponent<UITarget>();
            public bool Interactable;
            public bool CPRAble;
        }
        [SerializeField] List<TargetClass> _targetObjects;

        void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
            foreach (RectTransform rect in _uiTargetsPool)
                rect.gameObject.SetActive(false);
        }
        void Start()
        {
            foreach (TargetClass target in _targetObjects)
                target.AssignUITarget();
            SwitchToCPRSprites(false);
            DisableTargets();
        }
        public RectTransform GetTargetTransform()
        {
            for(int i = 0; i < _uiTargetsPool.Count; i++)
            {
                if (_uiTargetsPool[i].gameObject.activeSelf)
                    continue;
                else
                    return _uiTargetsPool[i];
            }
            return null;
        }
        public void SwitchToCPRSprites(bool inCprMode)
        {
            switch (inCprMode)
            {
                case true:
                    SwapSprite(_iCPRableUI);
                    break;
                case false:
                    SwapSprite(_iInteractableUI);
                    break;
            }
            TargetsCPRMode(inCprMode);
        }
        // use SwitchToCPRSprites() for Enabling
        public void DisableTargets()
        {
            foreach (TargetClass target in _targetObjects)
            {
                target.UITarget.enabled = false;
            }
        }
        private void TargetsCPRMode(bool inCprMode)
        {
            switch(inCprMode)
            {
                case true:
                    foreach (TargetClass target in _targetObjects)
                    {
                        if (target.CPRAble) target.UITarget.enabled = true;
                        else target.UITarget.enabled = false;
                    }
                    break;
                case false:
                    foreach (TargetClass target in _targetObjects)
                    {
                        if (target.Interactable) target.UITarget.enabled = true;
                        else target.UITarget.enabled = false;
                    }
                    break;
            }
        }
        private void SwapSprite(Sprite sprite)
        {
            foreach (RectTransform rect in _uiTargetsPool)
            {
                rect.GetComponent<Image>().sprite = sprite;
            }
        }
    }
}