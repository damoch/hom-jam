﻿using UnityEngine;

namespace Assets.Scripts
{
    public class MusicController : MonoBehaviour
    {
        public AudioClip MenuMusic;
        public AudioClip InGameMusic;

        [Range(0f,10f)]
        public float FadeInLength;

        private AudioSource _audioSource;
        private float _baseVolume;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _baseVolume = _audioSource.volume;
        }

        private void Update()
        {

        }

        public void PlayMenuMusic()
        {
            _audioSource.clip = MenuMusic;
            _audioSource.Play();
        }

        public void PlayInGameMusic()
        {
            _audioSource.clip = InGameMusic;
            _audioSource.Play();
        }
    }
}
