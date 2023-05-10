using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;
using CardiacPatient;

namespace PlayerControl {
    /// <summary>
    /// Player CPR Control When Patient is Assigned
    /// </summary>
    [RequireComponent(typeof(FirstPersonController))]
    [RequireComponent(typeof(StarterAssetsInputs))]
    [RequireComponent(typeof(PlayerInteraction))]
    public class PlayerCPRTargeting : MonoBehaviour
    {
        // When Patient == null, disable script. Otherwise Script Runs
        [HideInInspector]
        public Patient patient;
        [HideInInspector]
        public Transform playerSit;

        [Tooltip("For Reference by PatientManager")]
        public PlayerHandModel handModel;
        private readonly WaitForSeconds enabledDelay = new(1.5f);

        [System.Serializable]
        struct AttendEvents
        {
            public UnityEvent eventStartAttendingPatient;
            public UnityEvent eventStopAttendingPatient;
        }
        [SerializeField] private AttendEvents _attendEvents;
        
        private FirstPersonController _fpsController;
        private StarterAssetsInputs _playerInput;
        private PlayerInteraction _playerInteraction;

        private void Start()
        {
            _fpsController = GetComponent<FirstPersonController>();
            _playerInput = GetComponent<StarterAssetsInputs>();
            _playerInteraction = GetComponent<PlayerInteraction>();
        }
        void FixedUpdate()
        {
            if (patient == null)
            {
                enabled = false;
                return;
            }
            if (playerSit != null)
            {
                if (transform.position != playerSit.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, playerSit.position, .01f);
                }
            }
        }
        private IEnumerator CPRControlMode(bool on)
        {
            if (on)
            {
                _fpsController.enabled = false;
                _playerInput.enabled = false;
                _playerInteraction.enabled = false;
            }
            else
            {
                _fpsController.enabled = true;
                _playerInput.enabled = true;
                yield return enabledDelay;
                _playerInteraction.enabled = true;
            }
            yield return null;
        }

        // Manage Enabling and Disabling FPS Controller
        public void SetPatient(Patient patient)
        {
            if (patient == null)
            {
                playerSit = null;
                _attendEvents.eventStopAttendingPatient.Invoke();

                this.patient.GetComponent<CardiacPatientManager>().SetupCPRInteraction();

                StartCoroutine(nameof(CPRControlMode), false);
            }
            else
            {
                _attendEvents.eventStartAttendingPatient.Invoke();
                
                enabled = true;

                StartCoroutine(nameof(CPRControlMode), true);
            }
            this.patient = patient;
        }
    }
}