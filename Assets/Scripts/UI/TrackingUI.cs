using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PlayerControl;

public class TrackingUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    RectTransform _rect;
    GameObject _textObject;
    float _originalScale;

    ICPRable _cPRable;
    readonly float _growTarget = 1.5f;
    readonly float _growSpeed = 2f;
    bool _grow = false;
    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _textObject = transform.GetChild(0).gameObject;
        _textObject.SetActive(false);
        _originalScale = _rect.localScale.x;
    }
    void OnEnable()
    {
        if (_textObject == null) return;
        _textObject.SetActive(false);
        _grow = false;
    }
    void FixedUpdate()
    {
        switch (_grow)
        {
            case true:
                Grow(_rect, _growTarget);
                break;
            case false:
                Shrink(_rect, _originalScale);
                break;
        }
    }
    void Grow(RectTransform rectTransform, float targetScale)
    {
        if (rectTransform.localScale.x < targetScale)
        {
            rectTransform.localScale += Time.deltaTime * _growSpeed * Vector3.one;
        }
    }
    void Shrink(RectTransform rectTransform, float targetScale)
    {
        if (rectTransform.localScale.x > targetScale)
        {
            rectTransform.localScale -= Time.deltaTime * _growSpeed * Vector3.one;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _textObject.SetActive(false);
        _grow = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _textObject.SetActive(true);
        _grow = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        InteractCPR();
    }
    public void AssignCPRAble(ICPRable cPRable)
    {
        _cPRable = cPRable;
    }
    void InteractCPR()
    {
        _cPRable.Interact(GlobalInstance.Instance.MainCPRMgr);
    }
    public void Disallow()
    {
        _rect.localScale = Vector3.one;
        enabled = false;
    }
    public void Allow()
    {
        enabled = true;
    }
}
