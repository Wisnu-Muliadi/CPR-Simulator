using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardiacPatient
{
    [RequireComponent(typeof(CardiacPatientStats), typeof(CardiacPatientLogic))]
    //[RequireComponent(typeof(CardiacPatientManager), typeof(CardiacPatientAnimator), typeof(CardiacPatientMuscles))]
    public class Patient : MonoBehaviour
    {
        [HideInInspector]
        public CardiacPatientStats cardiacPatientStats;
        [HideInInspector]
        public CardiacPatientLogic cardiacPatientLogic;

        void Start()
        {
            cardiacPatientStats = GetComponent<CardiacPatientStats>();
            cardiacPatientLogic = GetComponent<CardiacPatientLogic>();
        }
    }
}