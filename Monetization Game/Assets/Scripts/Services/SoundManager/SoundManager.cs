using System.Collections.Generic;
using UnityEngine;

namespace Services.SoundManager
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> _soundClips;
        private AudioSource _audioSource;
        private AudioSource _musicSource;
        private bool _soundOn;

        public void Initialize()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _musicSource = gameObject.AddComponent<AudioSource>();
        }

        public void PlaySound(int index)
        {
            if(!CheckSoundStatus())
                return;
            
            _audioSource.clip = _soundClips[index];
            _audioSource.Play();
        }
        
        public void PlayMusic(int index)
        {
            if(_musicSource.isPlaying || !CheckMusicStatus())
                return;
            _musicSource.loop = true;
            _musicSource.clip = _soundClips[index];
            _musicSource.Play();
        }

        public void StopAll()
        {
            if(_musicSource.isPlaying)
                _musicSource.Stop();
            if(_audioSource.isPlaying)
                _audioSource.Stop();
        }

        private bool CheckSoundStatus()
        {
            var soundOn = PlayerPrefs.GetInt("SoundOn");

            if (soundOn == 0)
                return false;
            else
                return true;
        }
        
        private bool CheckMusicStatus()
        {
            var soundOn = PlayerPrefs.GetInt("MusicOn");

            if (soundOn == 0)
                return false;
            else
                return true;
        }
    }
}
