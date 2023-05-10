using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CardiacPatient
{
    [CustomEditor(typeof(CardiacPatientLogic))]
    public class CardiacPatientLogicTest : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CardiacPatientLogic logic = (CardiacPatientLogic)target;
            if (GUILayout.Button("Resuscitate"))
            {
                logic.HandleResuscitation(1f, 1f);
            }
            if (GUILayout.Button("Give Oxygen"))
            {
                logic.HandleGiveOxygen(1, false);
            }
        }
    }
}