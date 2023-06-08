using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerControl;

namespace UserInterface {
    public class InteractTargetManager : MonoBehaviour
    {
        public static InteractTargetManager Instance;
        //[Tooltip("Use SetActiveUiTargets() to enable specific target")]public bool TutorialOverride = false;

        [SerializeField] bool _startShowUI = false;
        [SerializeField] Sprite _iInteractableUI, _iCPRableUI;
        [SerializeField] List<RectTransform> _uiTargetsPool;
        [SerializeField] SkinnedMeshRenderer _patientRenderer;

        [System.Serializable]
        class TargetClass
        {
            public GameObject TargetObject;
            public UITarget UITarget;
            public void AssignUITarget(SkinnedMeshRenderer targetRenderer)
            {
                UITarget = TargetObject.AddComponent<UITarget>();
                UITarget.patientRenderer = targetRenderer;
            }
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
                target.AssignUITarget(_patientRenderer);
            SwitchToCPRSprites(false);
            if(!_startShowUI) DisableTargets();
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
        /*public void SetActiveUiTarget(int index)
        {
            _targetObjects[index].UITarget.enabled = true;
        }*/
        public void UpdateCPRAbleTargets()
        {
            foreach (TargetClass target in _targetObjects)
            {
                target.UITarget.enabled = target.CPRAble;
            }
        }
        public void InteractableUiTarget(int index)
        {
            _targetObjects[index].Interactable = true;
            //_targetObjects[index].UITarget.enabled = true;
        }
        public void CprAbleUiTarget(int index)
        {
            _targetObjects[index].CPRAble = true;
            //_targetObjects[index].UITarget.enabled = true;
        }
        public void DisableAllTarget()
        {
            foreach (TargetClass target in _targetObjects)
            {
                target.Interactable = false;
                target.CPRAble = false;
                target.UITarget.enabled = false;
            }
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
            //if (TutorialOverride) return;
            foreach (TargetClass target in _targetObjects)
            {
                switch (inCprMode)
                {
                    case true:
                        target.UITarget.enabled = target.CPRAble;
                        break;
                    case false:
                        target.UITarget.enabled = target.Interactable;
                        break;
                }
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