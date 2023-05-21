using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CPR_Camera;

namespace PlayerControl
{
    public interface ICPRable
    {
        void Interact(CPRMainManager cprMainControl);
        string GetDescription();
    }
    public class CPRMainManager : MonoBehaviour
    {
        [HideInInspector] public CPR_CamPasser cprCamPasser;
        [HideInInspector] public CPR_CamController cprCamCtrl;
        [HideInInspector] public PlayerCPRTargeting cprTargeting;

        [SerializeField] SkinnedMeshRenderer _playerHands;
        public UnityEvent<string> eventUI_Update;
        public UnityEvent<bool> eventUI_Enable;
        [SerializeField, Tooltip("CPR Push depth first in!")] List<GameObject> _bpmUI = new();
        [SerializeField] GameObject _giveBreathUI; // anybetterway?
        public enum CamState
        {
            Overview,
            Head,
            Chest,
            BreathSupport
        }
        public CamState camState = CamState.Overview;
        private CamState _prevState;
        [System.Serializable]
        public struct CamChangeState
        {
            public UnityEvent onEnterOverview, onEnterHead, onEnterChest, onBreathSupport;
        }
        [SerializeField] CamChangeState _camChangeState;

        private Camera thisCam;
        private Vector3 mousePos;
        private Vector2 offset;
        private Ray mouseRay;
        private RaycastHit hitInfo;
        [SerializeField] LayerMask layerMask;

        void OnEnable()
        {
            _playerHands.enabled = true;
        }
        void OnDisable()
        {
            _playerHands.enabled = false;
        }
        void Start()
        {
            thisCam = GetComponent<Camera>();
            _prevState = camState;
            foreach (GameObject gObj in _bpmUI)
                gObj.SetActive(false);
            if (_giveBreathUI != null) _giveBreathUI.SetActive(false);
        }
        void Update()
        {
            if (cprCamPasser == null) { enabled = false; return; }
            UpdateState();
            CheckExit();
        }
        private void UpdateState()
        {
            switch (camState)
            {
                case CamState.Overview:
                    HandleSelection();
                    break;
                case CamState.Head:
                    if (!cprCamPasser.ActiveCam(1))
                    {
                        cprCamPasser.ActivateCam(1, true);
                        cprCamCtrl.StartCoroutine(nameof(cprCamCtrl.IRealignCprRoot));
                        StartCoroutine(ICheckingHead()); //last minute addition. should do better than this
                        _camChangeState.onEnterHead.Invoke();
                    }
                    break;
                case CamState.Chest:
                    if (!cprCamPasser.ActiveCam(2))
                    {
                        cprCamPasser.ActivateCam(2, true);
                        foreach (GameObject gObj in _bpmUI)
                            gObj.SetActive(true);
                        cprCamCtrl.StartCoroutine(nameof(cprCamCtrl.IRealignCprRoot));
                        _camChangeState.onEnterChest.Invoke();
                    }
                    break;
                case CamState.BreathSupport:
                    if (!cprCamPasser.ActiveCam(3))
                    {
                        cprCamPasser.ActivateCam(3, true);
                        cprCamCtrl.StartCoroutine(nameof(cprCamCtrl.IRealignCprRoot));
                        if (_giveBreathUI != null)
                        {
                            _giveBreathUI.SetActive(true);
                            _giveBreathUI.GetComponentInChildren<UserInterface.GiveBreathUILogic>().ButtonScriptActive(true);
                        }
                        _camChangeState.onBreathSupport.Invoke();
                    }
                    break;
                default:
                    break;
            }
        }
        private void HandleSelection()
        {
            mouseRay = thisCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out hitInfo, layerMask))
            {
                ICPRable selectable = hitInfo.collider.GetComponent<ICPRable>();
                if (selectable == null) 
                {
                    eventUI_Enable.Invoke(false);
                }
                else
                {
                    eventUI_Enable.Invoke(true);
                    eventUI_Update.Invoke(selectable.GetDescription());
                    if (Input.GetButtonDown("Interact") || Input.GetMouseButtonDown(0))
                    {
                        selectable.Interact(this);
                        eventUI_Enable.Invoke(false);
                    }
                }
            }
            else
            {
                eventUI_Enable.Invoke(false);
            }
        }
        private void CheckExit()
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (camState != CamState.Overview && camState != CamState.Head)
                {
                    ReturnToOverview();
                }
                else if (camState == CamState.Head)
                {
                    return;
                }
                else
                {
                    cprTargeting.SetPatient(null);
                }
                eventUI_Enable.Invoke(false);
            }
        }
        private void ReturnToOverview()
        {
            camState = CamState.Overview;
            cprCamPasser.ActivateCam(1, false);
            cprCamPasser.ActivateCam(2, false);
            cprCamPasser.ActivateCam(3, false);
            foreach (GameObject gObj in _bpmUI)
                gObj.SetActive(false);
            if (_giveBreathUI != null) _giveBreathUI.SetActive(false);
            _camChangeState.onEnterOverview.Invoke();
        }
        public void SetCPRCam(CPR_CamPasser camPasser)
        {
            if (camPasser == null)
            {
                enabled = false;
            }
            else
            {
                enabled = true;
            }
            cprCamPasser = camPasser;
        }
        public bool StateChanged()
        {
            if (_prevState != camState)
            {
                _prevState = camState;
                return true;
            }
            return false;
        }
        private IEnumerator ICheckingHead()
        {
            try { GlobalInstance.Instance.UIManager.loadingCircle.SetActive(true); }
            catch { }
            yield return new WaitForSecondsRealtime(4f);
            ReturnToOverview();
        }
    }
}