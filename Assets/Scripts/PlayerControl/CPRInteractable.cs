using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CardiacPatient;

namespace PlayerControl
{
    public class CPRInteractable : MonoBehaviour, IInteractable
    {
        public UnityAction InteractAction { get; set; }
        public Patient patient;
        string returnedText;
        public void Interact(PlayerInteraction player)
        {
            if (patient.TryGetComponent(out CardiacPatientManager cpManager))
            {
                cpManager.SetupCPRInteraction(player);
                InteractAction?.Invoke();
            }
        }
        public string GetDescription()
        {
            returnedText = "Periksa Pasien";
            return returnedText;
        }
    }
}