using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using PlayerControl;

public class TutorialSwitcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] bool _tutorialModeOnStart = true;
    [SerializeField] EnterLevelScene _enterLevelScene;
    [SerializeField] ModelInteract _boy, _girl;
    [Header("Button")]
    [SerializeField] Image _buttonImage;
    [SerializeField] Sprite _highlightSprite;
    [SerializeField] Sprite _tutorialModeSprite;
    [SerializeField] Sprite _simulationModeSprite;
    [Header("Adjustments")]
    [SerializeField] UnityEvent _tutorialMode = new();
    [SerializeField] UnityEvent _simulationMode = new();

    bool _inSimulationMode = false;
    private void Start()
    {
        if (_tutorialModeOnStart)
        {
            _boy.gameObject.SetActive(false);
            _girl.gameObject.SetActive(true);
            _enterLevelScene.TargetGameSceneIndex = 3;
            _inSimulationMode = false;
            _tutorialMode.Invoke();
        }
        else
        {
            _boy.gameObject.SetActive(true);
            _girl.gameObject.SetActive(false);
            _enterLevelScene.TargetGameSceneIndex = 2;
            _inSimulationMode = true;
            _simulationMode.Invoke();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _buttonImage.sprite = _highlightSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_inSimulationMode) _buttonImage.sprite = _simulationModeSprite;
        else _buttonImage.sprite = _tutorialModeSprite;
    }

    public void Toggle()
    {
        if (_boy.gameObject.activeSelf)
        {
            _boy.Interact();
            _inSimulationMode = false;
            _tutorialMode.Invoke();
        }
        else
        {
            _girl.Interact();
            _inSimulationMode = true;
            _simulationMode.Invoke();
        }
    }
}
