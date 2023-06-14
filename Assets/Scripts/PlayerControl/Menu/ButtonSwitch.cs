using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using PlayerControl;

public class ButtonSwitch : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Button")]
    [SerializeField] Image _buttonImage;
    [SerializeField] Sprite _highlightSprite;
    [SerializeField] Sprite _modeASprite;
    [SerializeField] Sprite _modeBSprite;
    [Header("Switched Event")]
    [SerializeField] protected UnityEvent _modeA = new();
    [SerializeField] protected UnityEvent _modeB = new();

    protected bool _currentModeA = true;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _buttonImage.sprite = _highlightSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_currentModeA) _buttonImage.sprite = _modeASprite;
        else _buttonImage.sprite = _modeBSprite;
    }

    public void Toggle()
    {
        if (_currentModeA)
        {
            _modeB.Invoke();
        }
        else
        {
            _modeA.Invoke();
        }
        _currentModeA = !_currentModeA;
    }
}
