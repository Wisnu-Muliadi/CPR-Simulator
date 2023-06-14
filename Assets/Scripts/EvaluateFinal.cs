using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvaluateFinal : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _finalGradeText;
    [SerializeField] List<Image> _checks;
    [SerializeField] Sprite _true, _false;
    int _checksValue = 0;
    [SerializeField] CardiacPatient.CardiacPatientStats _patientStats;

    private void Start()
    {
        for (int i = 0; i < _checks.Count; i++)
        {
            _checks[i].sprite = _false;
        }
        _checksValue = 0;
        gameObject.SetActive(false);
    }
    public void Evaluation()
    {
        if (_patientStats.PatientHealthPercentage() > .5f)
        { _checks[0].sprite = _true; _checksValue++; }
        
        if (_patientStats.PatientHealthPercentage() == 1f)
        { _checks[1].sprite = _true; _checksValue++; }

        if(_patientStats.PatientOxygenPercentage() >= .6f)
        { _checks[2].sprite = _true; _checksValue++; }

        switch (_checksValue)
        {
            case 0:
                _finalGradeText.text = "F"; _finalGradeText.color = Color.red;
                break;
            case 1:
                _finalGradeText.text = "C"; _finalGradeText.color = Color.gray;
                break;
            case 2:
                _finalGradeText.text = "B"; _finalGradeText.color = Color.blue;
                break;
            case 3:
                _finalGradeText.text = "A"; _finalGradeText.color = Color.yellow;
                break;
        }
    }
}
