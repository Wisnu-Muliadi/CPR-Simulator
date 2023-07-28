using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using CPR_Camera;

namespace PlayerControl
{
    public interface ICPRable
    {
        UnityAction InteractAction { get; set; }
        void Interact(CPRMainManager cprMainControl);
        string GetDescription();
    }
    public class CPRMainManager : MonoBehaviour
    {
        [HideInInspector] public CPR_CamPasser cprCamPasser;
        [HideInInspector] public CPR_CamController cprCamCtrl;
        [HideInInspector] public PlayerCPRTargeting cprTargeting;
        public bool disableExit = false;
        public void InvokeExit() => Exit();

        [SerializeField] SkinnedMeshRenderer _playerHands;
        public UnityEvent<string> eventUI_Update;
        public UnityEvent<bool> eventUI_Enable;
        [SerializeField, Tooltip("CPR Push depth first in!")] List<GameObject> _bpmUI = new();
        [SerializeField] Image _pulseAnimImage;
        [SerializeField] Image _bpmSliderHelper;
        bool _expired = false;
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
            camState = CamState.Overview;
            if(cprCamPasser != null)
            {
                cprCamPasser.ActivateCam(1, false);
                cprCamPasser.ActivateCam(2, false);
                cprCamPasser.ActivateCam(3, false);
            }
            if (_playerHands == null) return; // bandage. don't keep
            _playerHands.enabled = false;
        }
        void Start()
        {
            thisCam = GetComponent<Camera>();
            _prevState = camState;
            EnableCPRUI(false);
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
                    //HandleSelection(); // [Deprecated, Changed to using UIs]
                    break;
                case CamState.Head:
                    if (!cprCamPasser.ActiveCam(1))
                    {
                        cprCamPasser.ActivateCam(1, true);
                        cprCamCtrl.StartCoroutine(nameof(cprCamCtrl.IRealignCprRoot));
                        StartCoroutine(ICheckingHead()); //sequence check nafas pasien. last minute mess
                        _camChangeState.onEnterHead.Invoke();
                    }
                    break;
                case CamState.Chest:
                    if (!cprCamPasser.ActiveCam(2))
                    {
                        cprCamPasser.ActivateCam(2, true);
                        EnableCPRUI(true);
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
        /*
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
        }*/
        private void CheckExit()
        {
            if (disableExit) return;
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Phone"))
            {
                Exit();
            }
        }
        private void Exit()
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
        public void Expired(bool expired)
        {
            _expired = expired;
        }
        public void DisableExit(bool disable)
        {
            disableExit = disable;
        }
        public void EnableCPRUI(bool enable)
        {
            foreach (GameObject gObj in _bpmUI)
                gObj.SetActive(enable);
            _pulseAnimImage.enabled = enable && !_expired;
            _bpmSliderHelper.enabled = enable && !_expired;
        }
        private void ReturnToOverview()
        {
            camState = CamState.Overview;
            cprCamPasser.ActivateCam(1, false);
            cprCamPasser.ActivateCam(2, false);
            cprCamPasser.ActivateCam(3, false);
            EnableCPRUI(false);
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