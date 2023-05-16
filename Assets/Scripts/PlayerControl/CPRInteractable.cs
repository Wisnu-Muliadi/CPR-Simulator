using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardiacPatient;

namespace PlayerControl
{
    public class CPRInteractable : MonoBehaviour, IInteractable
    {
        public Patient patient;
        string returnedText;
        public void Interact(PlayerInteraction player)
        {
            if (patient.TryGetComponent(out CardiacPatientManager cpManager))
            {
                cpManager.SetupCPRInteraction(player);
            }
        }
        public string GetDescription()
        {
            returnedText = "Periksa Pasien";
            return returnedText;
        }
    }
}