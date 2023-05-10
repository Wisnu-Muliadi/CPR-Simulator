using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CardiacPatient
{
    [CustomEditor(typeof(CardiacPatientManager))]
    public class PatientEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CardiacPatientManager patient = (CardiacPatientManager)target;
            if (GUILayout.Button("DeployIInteractable"))
            {
                patient.DeployInteractable();
            }

        }
    }
}