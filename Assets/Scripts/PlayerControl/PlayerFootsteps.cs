using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

namespace PlayerControl
{
    public class PlayerFootsteps : MonoBehaviour
    {
        private StarterAssetsInputs _starterAssetsInputs;
        private FirstPersonController _fpsController;
        private Vector2 travelled = new();

        private AudioSource _audioSource;
        [SerializeField] List<AudioClip> _stepSounds;
        [SerializeField] AudioClip _jumpSound;

        bool onGround = false;
        void Start()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.spatialBlend = 1;
            _audioSource.volume = .8f;
            _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
            _fpsController = GetComponent<FirstPersonController>();
        }

        void Update()
        {
            travelled += _starterAssetsInputs.move * Time.deltaTime;
            if (onGround != _fpsController.Grounded)
            {
                PlayJumpSound();
                onGround = _fpsController.Grounded;
            }
            if (!_fpsController.Grounded) return;
            else if (travelled.magnitude > .4f)
            {
                PlayStepSound();
                travelled = Vector2.zero;
            }
        }
        void PlayStepSound()
        {
            //_audioSource.clip = _stepSounds[Random.Range(0, _stepSounds.Count)];
            _audioSource.PlayOneShot(_stepSounds[Random.Range(0, _stepSounds.Count)]);
        }
        void PlayJumpSound()
        {
            _audioSource.PlayOneShot(_jumpSound);
        }
    }
}