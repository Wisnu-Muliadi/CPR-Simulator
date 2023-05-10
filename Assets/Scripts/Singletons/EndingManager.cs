using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using CardiacPatient;

public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance { get; private set; }
    [SerializeField] PlayableDirector _failPlayable;
    [SerializeField] CardiacPatientManager _caManager;
    [SerializeField] GameObject[] _disableOnEnding;

    Transform _chestTransform;
    void Start()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    public void FailedEnding()
    {
        _failPlayable.gameObject.SetActive(true);
        _chestTransform = _caManager.ChestCollider.transform;
        Vector3 endingPosition = _chestTransform.position;
        Quaternion endingRotation = Quaternion.LookRotation(-_chestTransform.transform.up, _failPlayable.transform.up);
        _failPlayable.transform.SetPositionAndRotation(endingPosition, endingRotation);
        if (_caManager.InstancedCamera != null)
            _caManager.InstancedCamera.SetActive(false);
        _failPlayable.Play();
        for (int i = 0; i < _disableOnEnding.Length; i++)
        {
            _disableOnEnding[i].SetActive(false);
        }
    }
}
