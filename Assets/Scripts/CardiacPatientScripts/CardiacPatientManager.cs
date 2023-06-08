using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using CPR_Camera;
using PlayerControl;
using UserInterface;

namespace CardiacPatient
{
    [RequireComponent(typeof(Patient))]
    public class CardiacPatientManager : MonoBehaviour
    {
        [SerializeField]
        private Patient _patient;
        // CPR Mode Instances
        [SerializeField, Tooltip("Will be Instanced on the First Object in the \"Chest Colliders\" Array")]
        private GameObject _cPRCameras;
        [HideInInspector] public GameObject InstancedCamera;
        [SerializeField, Tooltip("Also Instanced on First Chest Colliders")]
        private GameObject _chestIKRoot;
        [SerializeField] Vector3 _chestOffset = Vector3.zero;
        [SerializeField, Tooltip("Instanced on Head Collider")]
        private GameObject _headIKRoot;
        [SerializeField] Vector3 _headOffset = Vector3.zero;
        readonly private GameObject[] _instancedIKRoot = new GameObject[2];

        private AnimationController.AnimatorController _animatorController;
        private AnimationController.PuppetMasterController _puppetController;
        // Patient's Parts
        [SerializeField] GameObject[] _interactableColliders, _chestColliders, _armsColliders;
        [HideInInspector] public GameObject ChestCollider;
        public GameObject headCollider;
        [Space]
        [SerializeField] HealthUI _healthUI;
        [SerializeField] CprButtonUILogic _cprButtonUILogic;
        [SerializeField] GiveBreathUILogic _giveBreathUILogic;

        [SerializeField] bool _deployInteractablesOnStart = false;

        private void Awake()
        {
            if (_deployInteractablesOnStart) DeployInteractable();
        }
        private void Start()
        {
            _animatorController = _patient.GetComponentInChildren<AnimationController.AnimatorController>();
            _puppetController = _patient.GetComponentInChildren<AnimationController.PuppetMasterController>();
            // Positioning IK Instances
            _instancedIKRoot[0] = Instantiate(_chestIKRoot, _chestColliders[0].transform);
            _instancedIKRoot[0].transform.SetLocalPositionAndRotation(_chestOffset, Quaternion.Euler(0, 0, 0));
            _instancedIKRoot[1] = Instantiate(_headIKRoot, headCollider.transform);
            _instancedIKRoot[1].transform.SetLocalPositionAndRotation(_headOffset, Quaternion.Euler(0, 0, 0));

            ChestCollider = _chestColliders[0];
        }

        public void SetupCPRInteraction(PlayerInteraction player)
        {
            StartCoroutine(nameof(ISetupCPRInteractionRoutine), player);
        }
        public void SetupCPRInteraction()
        {
            StartCoroutine(nameof(IResetCPRInteraction));
        }
        // smelly messy spaghetti here
        private IEnumerator ISetupCPRInteractionRoutine(PlayerInteraction player)
        {
            GlobalInstance.Instance.UIManager.MouseDisplayAdd(true);
            if (TryGetComponent(out CardiacPatientMuscles cardiacPatientMuscles))
                cardiacPatientMuscles.StartAttend();
            if(_animatorController) _animatorController.animator.SetBool("Attended", true);
            GameObject upperChest = _chestColliders[0];
            // Instance Camera to Patient
            Rigidbody chestRigidbody = upperChest.GetComponent<Rigidbody>();
            Vector3 torque;
            Quaternion rot;
            float faceUpTime = 0;
            float faceUpDuration = 2;
            do
            {
                rot = Quaternion.FromToRotation(upperChest.transform.forward, transform.up);
                torque = Mathf.Rad2Deg * Quaternion.ToEulerAngles(rot);
                chestRigidbody.AddTorque(.25f * torque, ForceMode.VelocityChange);
                if (Vector3.Dot(upperChest.transform.forward, transform.up) > .95f) faceUpTime += Time.deltaTime;
                
                if (InstancedCamera == null)
                {
                    InstancedCamera = Instantiate(_cPRCameras, upperChest.transform, true);
                    Vector3 rootPosition = upperChest.transform.position;
                    Quaternion rootRotation = Quaternion.LookRotation(-upperChest.transform.up, InstancedCamera.transform.up);
                    InstancedCamera.transform.SetPositionAndRotation(rootPosition, rootRotation);
                    // Set CPR Controls to Player
                    if (player.TryGetComponent(out PlayerCPRTargeting playerCPR))
                    {
                        playerCPR.SetPatient(_patient);
                        playerCPR.handModel.chestIkTarget = new Transform[] { _instancedIKRoot[0].transform.GetChild(0), _instancedIKRoot[0].transform.GetChild(1)};
                        playerCPR.handModel.headIkTarget = new Transform[] { _instancedIKRoot[1].transform.GetChild(0), _instancedIKRoot[1].transform.GetChild(1)};

                        if (InstancedCamera.TryGetComponent(out CPR_CamController cprCamCon))
                        {
                            cprCamCon.CheckRightLeftCam(playerCPR);
                            // Player on Patient's Left
                            if (cprCamCon.PlayerOnLeftSide(playerCPR) > 0)
                            {
                                _instancedIKRoot[0].transform.Rotate(0, 0, 180, Space.Self);
                                playerCPR.handModel.headIkTarget = new Transform[] { _instancedIKRoot[1].transform.GetChild(1), _instancedIKRoot[1].transform.GetChild(0) };
                                _instancedIKRoot[1].transform.localScale = new Vector3(-1, 1, 1);
                                playerCPR.handModel.MirroredAnim(true);
                            }
                            else
                            {
                                playerCPR.handModel.headIkTarget = new Transform[] { _instancedIKRoot[1].transform.GetChild(0), _instancedIKRoot[1].transform.GetChild(1) };
                                _instancedIKRoot[1].transform.localScale = new Vector3(1, 1, 1);
                                playerCPR.handModel.MirroredAnim(false);
                            }

                        }
                        if (GlobalInstance.Instance.mainCam.TryGetComponent(out CPRMainManager cprMainMngr))
                        {
                            cprMainMngr.SetCPRCam(InstancedCamera.GetComponent<CPR_CamPasser>());
                            cprMainMngr.cprTargeting = playerCPR;
                            cprMainMngr.cprCamCtrl = cprCamCon;
                        }
                    }
                    if (_healthUI != null) _healthUI.SetPatient(_patient);
                    if (_cprButtonUILogic != null) _cprButtonUILogic.SetPatient(_patient);
                    if (_giveBreathUILogic != null) _giveBreathUILogic.SetPatient(_patient);
                }
                yield return null;
            } while (faceUpTime < faceUpDuration);
            
        }
        private IEnumerator IResetCPRInteraction()
        {
            StopCoroutine(nameof(ISetupCPRInteractionRoutine));
            GlobalInstance.Instance.UIManager.MouseDisplayAdd(false);
            GlobalInstance.Instance.mainCam.GetComponent<CPRMainManager>().SetCPRCam(null);
            if (TryGetComponent(out CardiacPatientMuscles cardiacPatientMuscles))
                cardiacPatientMuscles.StopAttend();
            if(_animatorController) _animatorController.animator.SetBool("Attended", false);
            if (_healthUI != null) _healthUI.SetPatient(null);

            CinemachineVirtualCamera[] cprVirtualCams = InstancedCamera.GetComponentsInChildren<CinemachineVirtualCamera>();
            foreach (CinemachineVirtualCamera virtualCam in cprVirtualCams)
            {
                virtualCam.Priority -= 5;
            }
            yield return new WaitForSeconds(1);

            // Blend back to Player Cam
            CinemachineBrain cinemachineBrain = GlobalInstance.Instance.mainCam.GetComponent<CinemachineBrain>();
            if (cinemachineBrain != null)
            {
                while (cinemachineBrain.IsBlending)
                {
                    yield return null;
                }
            }
            _instancedIKRoot[0].transform.localEulerAngles = Vector3.zero;
            Destroy(InstancedCamera);
        }

        public void DeployInteractable()
        {
            foreach (GameObject col in _interactableColliders)
            {
                col.AddComponent<CPRInteractable>().patient = _patient;
            }
            foreach (GameObject col in _chestColliders)
            {
                col.AddComponent<CPRableChest>();
            }
            foreach (GameObject col in _armsColliders)
            {
                col.AddComponent<CPRableArm>();
            }
            headCollider.AddComponent<CPRableHead>();
        }
        public void DestroyInteractables()
        {
            foreach (GameObject col in _interactableColliders)
            {
                Destroy(col.GetComponent<CPRInteractable>());
            }
        }
    }
}