using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CardiacPatient;

namespace UserInterface
{
    public class HealthUI : MonoBehaviour
    {
        [HideInInspector]
        public Patient patient;
        private CardiacPatientStats _patientStats;

        [SerializeField]
        private GameObject _healthBar, _oxygenBar;
        private Image _decreaseIconUI;
        private TextMeshProUGUI _decreaseCount;
        [SerializeField]
        private Sprite _decreaseIcon, _stagnantIcon;
        private Slider _healthSlider, _oxygenSlider;

        // Start is called before the first frame update
        void Start()
        {
            _healthSlider = _healthBar.GetComponent<Slider>();
            _oxygenSlider = _oxygenBar.GetComponent<Slider>();

            Transform decreaseCountGroup = _oxygenBar.transform.GetChild(3);
            _decreaseIconUI = decreaseCountGroup.GetChild(0).GetComponent<Image>();
            _decreaseCount = decreaseCountGroup.GetChild(1).GetComponent<TextMeshProUGUI>();

        }
        void Update()
        {
            if (patient == null || _patientStats == null)
            {
                enabled = false;
                _healthBar.SetActive(false);
                _oxygenBar.SetActive(false);
                return;
            }
            UpdateOxygenUI();
            if (_patientStats.PatientHealthPercentage() < 1f)
                UpdateHealthUI();
        }
        void FixedUpdate()
        {
            if(patient != null && _patientStats != null)
                UpdateDecreaseCounter();
        }
        private void UpdateHealthUI()
        {
            if (!_healthBar.activeSelf)
            {
                _healthBar.SetActive(true);
            }
            _healthSlider.value = _patientStats.PatientHealthPercentage();
        }
        private void UpdateOxygenUI()
        {
            if (!_oxygenBar.activeSelf)
            {
                _oxygenBar.SetActive(true);
            }
            _oxygenSlider.value = _patientStats.PatientOxygenPercentage();
        }
        private void UpdateDecreaseCounter()
        {
            _decreaseCount.text = _patientStats.SuffocationSpeed.ToString("0.##");
            if (_patientStats.SuffocationSpeed > 0)
            {
                _decreaseIconUI.sprite = _decreaseIcon;
            }
            else
            {
                _decreaseIconUI.sprite = _stagnantIcon;
            }
        }

        public void SetPatient(Patient patient)
        {
            if (patient != null && this.patient == null)
            {
                enabled = true;
                _patientStats = patient.cardiacPatientStats;
            }
            this.patient = patient;
        }
    }
}