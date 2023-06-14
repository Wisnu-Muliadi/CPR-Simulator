using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardiacPatient
{
    [RequireComponent(typeof(CardiacPatientManager))]
    public class PatientGasps : MonoBehaviour
    {
        private AudioSource _audioSource;
        private GameObject _headCollider;
        [SerializeField] AudioClip _gaspSound;
        void Start()
        {
            _headCollider = GetComponent<CardiacPatientManager>().headCollider;
            _audioSource = _headCollider.AddComponent<AudioSource>();
            _audioSource.spatialBlend = 1;
            _audioSource.volume = .6f;
        }

        public void GaspSound()
        {
            _audioSource.PlayOneShot(_gaspSound);
        }
        public void SetActiveSound(bool enabled)
        {
            _audioSource.enabled = enabled;
        }
    }
}