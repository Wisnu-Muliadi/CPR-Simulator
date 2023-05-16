using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using CardiacPatient;

public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance { get; private set; }
    [SerializeField] PlayableDirector _failPlayable;
    [SerializeField] PlayableDirector _successPlayable;
    [SerializeField] CardiacPatientManager _caManager;
    [SerializeField] GameObject[] _disableOnEnding;

    Transform _chestTransform;
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    public void FailedEnding()
    {
        _failPlayable.gameObject.SetActive(true);
        AlignEndingRoot(_failPlayable.transform, false);
        Disabling();
        _failPlayable.Play();
    }
    public void SuccessEnding()
    {
        _successPlayable.gameObject.SetActive(true);
        AlignEndingRoot(_successPlayable.transform, true);
        Disabling();
        _successPlayable.Play();
    }

    private void Disabling()
    {
        if (_caManager.InstancedCamera != null)
            _caManager.InstancedCamera.SetActive(false);
        for (int i = 0; i < _disableOnEnding.Length; i++)
        {
            _disableOnEnding[i].SetActive(false);
        }
    }
    private void AlignEndingRoot(Transform rootTransform, bool tweak)
    {
        _chestTransform = _caManager.ChestCollider.transform;
        Vector3 endingPosition = _chestTransform.position;
        Quaternion endingRotation = Quaternion.LookRotation(-_chestTransform.transform.up, rootTransform.up);
        endingPosition.y = 0;
        rootTransform.SetPositionAndRotation(endingPosition, endingRotation);
        if (tweak)
        {
            Quaternion rotation = _successPlayable.transform.rotation;
            _successPlayable.transform.localRotation = Quaternion.Euler(0, rotation.ToEuler().y * Mathf.Rad2Deg, 0);
        }
    }
}
