using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCamMovement : MonoBehaviour
{
    [SerializeField, Range(.05f, .5f)]
    private float offSetAmountY, offSetAmountX;

    private Quaternion _originalRot, _cameraRot;
    private Vector3 _mousePos;
    // Start is called before the first frame update
    void Start()
    {
        _originalRot = transform.localRotation;
        _cameraRot = _originalRot;
    }

    // Update is called once per frame
    void Update()
    {
        _mousePos = Input.mousePosition;
        transform.localRotation = Quaternion.Slerp(transform.localRotation,
            _cameraRot * Quaternion.Euler(-_mousePos.y * Mathf.Deg2Rad * offSetAmountY, _mousePos.x * Mathf.Deg2Rad * offSetAmountX, 0f), Time.deltaTime * 4);
    }
}
