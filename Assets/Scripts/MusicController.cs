using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

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
        private float _volumeDropPerSecond;
        private List<AudioClip> _customMusic;
        private int _currentTrackIndex;

        public string MusicFolderName;
        public string AllowedExtensionsPattern;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private async void TryLoadAudioFile()
        {
            _customMusic = new List<AudioClip>();
            var path = Path.Combine(Application.dataPath, MusicFolderName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return;
            }

            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                var ext = file.Split('.').Last();
                if(ext.ToLower() != AllowedExtensionsPattern)
                {
                    continue;
                }
                AudioClip clip = null;
                using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(file, AudioType.OGGVORBIS))
                {
                    uwr.SendWebRequest();

                    // wrap tasks in try/catch, otherwise it'll fail silently
                    try
                    {
                        while (!uwr.isDone) await Task.Delay(5);

                        if (uwr.isNetworkError || uwr.isHttpError) Debug.Log($"{uwr.error}");
                        else
                        {
                            clip = DownloadHandlerAudioClip.GetContent(uwr);
                            _customMusic.Add(clip);
                        }
                    }
                    catch (Exception err)
                    {
                        Debug.Log($"{err.Message}, {err.StackTrace}");
                    }
                }
            }
        }

        private async void Start()
        {
            _baseVolume = _audioSource.volume;
            TryLoadAudioFile();
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
            if(_customMusic.Count > 0)
            {
                if(_currentTrackIndex > _customMusic.Count - 1)
                {
                    _currentTrackIndex = 0;
                }
                _audioSource.clip = _customMusic[_currentTrackIndex++];
            }
            else
            {
                _audioSource.clip = InGameMusic;
            }
            _audioSource.Play();
        }
    }
}
