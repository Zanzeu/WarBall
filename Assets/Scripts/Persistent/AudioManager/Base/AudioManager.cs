using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using WarBall.Common;

public enum Sounds
{
    BallBounce,
    GridCollision,
    EnemyDeath,
}

namespace WarBall.Persistent
{
    public class AudioManager : PersistentSingleton<AudioManager>
    {   

        [LabelText("是否显示音效字典")] public bool ShowInspector = false;

        private const string PATH_BGMSO = "Audio/AudioData/AudioData_BGM";
        private const string PATH_SFXSO = "Audio/AudioData/AudioData_SFX";

        public float BGMVolume
        {
            get => _BGMVolume;
            set
            {
                _BGMVolume = value;
                onBGMVolumeChanged?.Invoke();

            }
        }

        public float SFXVolume
        {
            get => _SFXVolume;
            set
            {
                _SFXVolume = value;
                onSFXVolumeChanged?.Invoke();
            }
        }

        private event Action onBGMVolumeChanged;
        private event Action onSFXVolumeChanged;

        private AudioManagerSO BGMData;
        private AudioManagerSO SFXData;

        private List<AudioSource> _audioSources;

        private Dictionary<Sounds, AudioSource> _loopAudioSources;

        [ShowIf("ShowInspector")]
        [ShowInInspector]
        private Dictionary<Sounds, AudioClip[]> _audioClips;

        private float _BGMVolume;
        private float _SFXVolume;

        private GameObject _gameObject;

        protected override void Awake()
        {
            base.Awake();
            OnInit();
        }

        #region =====Logic Method=====

        private void OnInit()
        {
            InitDataFromSO();

            _audioSources = new List<AudioSource>();
            _loopAudioSources = new Dictionary<Sounds, AudioSource>();

            RegisterEvent();

            if (_gameObject == null)
            {
                _gameObject = new GameObject();
                _gameObject.name = GetType().ToString();
                _gameObject.transform.parent = transform;
            }
        }

        private void InitDataFromSO()
        {
            _audioClips = new Dictionary<Sounds, AudioClip[]>();

            BGMData = Resources.Load<AudioManagerSO>(PATH_BGMSO);
            _BGMVolume = BGMData.volume;

            SFXData = Resources.Load<AudioManagerSO>(PATH_SFXSO);
            _SFXVolume = SFXData.volume;

            foreach (var clips in BGMData.clips)
            {
                _audioClips.Add(clips.Key, clips.Value);
            }

            foreach (var clips in SFXData.clips)
            {
                _audioClips.Add(clips.Key, clips.Value);
            }
        }

        private void RegisterEvent()
        {
            onBGMVolumeChanged += UpdateBGMVolume;
            onSFXVolumeChanged += UpdateSXVolume;
        }

        private void RemoveSounds(Sounds sounds)
        {
            if (_loopAudioSources.ContainsKey(sounds))
            {
                _loopAudioSources[sounds].Stop();
                _loopAudioSources[sounds].clip = null;
                _loopAudioSources[sounds].loop = false;
                _loopAudioSources.Remove(sounds);
            }
        }

        private void RemoveAllSounds()
        {
            foreach (var audioSource in _loopAudioSources)
            {
                audioSource.Value.Stop();
                audioSource.Value.clip = null;
                audioSource.Value.loop = false;
            }

            _loopAudioSources.Clear();
        }

        private void PlaySounds(Sounds sound, bool loop = false)
        {
            var audioSource = GetAudioSource();

            if (loop)
            {
                RemoveSounds(sound);
                _loopAudioSources.Add(sound, audioSource);
            }

            AudioClip clip = GetClip(sound);
            if (clip == null)
            {
                Debug.LogError("音频缺失:" + sound);
                return;
            }

            audioSource.volume = loop ? _BGMVolume : _SFXVolume;
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }

        private AudioSource GetAudioSource()
        {
            foreach (var audioSource in _audioSources)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.loop = false;
                    return audioSource;
                }
            }

            return AddAudioSource();
        }

        private AudioSource AddAudioSource()
        {
            var audioSource = _gameObject.AddComponent<AudioSource>();
            _audioSources.Add(audioSource);

            return audioSource;
        }

        private AudioClip GetClip(Sounds name)
        {
            if (!_audioClips.ContainsKey(name))
            {
                return null;
            }

            return _audioClips[name][UnityEngine.Random.Range(0, _audioClips[name].Length)];
        }

        private void OnDie()
        {
            if (_gameObject)
            {
                Destroy(_gameObject);
            }

            onBGMVolumeChanged -= UpdateBGMVolume;
            onSFXVolumeChanged -= UpdateSXVolume;
        }

        #endregion

        #region =====Request Method=====
        public void PlayLoopRequest(Sounds soundName)
        {
            if (_loopAudioSources.ContainsKey(soundName))
            {
                return;
            }

            PlaySounds(soundName, true);
        }

        public void StopLoopRequest(Sounds soundName)
        {
            RemoveSounds(soundName);
        }

        public void StopAllLoopRequest()
        {
            RemoveAllSounds();
        }

        public void PlaySound(Sounds sound)
        {
            var audioSource = GetAudioSource();
            AudioClip clip = GetClip(sound);

            if (clip == null)
            {
                Debug.LogError("音频缺失:" + sound);
                return;
            }

            audioSource.volume = _SFXVolume;
            audioSource.clip = clip;
            audioSource.Play();
        }

        public void StopAllSounds()
        {
            foreach (var audioSource in _audioSources)
            {
                audioSource.Stop();
                audioSource.clip = null;
                audioSource.loop = false;
            }

            _loopAudioSources.Clear();
        }

        private void UpdateBGMVolume()
        {
            Debug.Log("调整音乐大小");

            foreach (var audioSource in _loopAudioSources)
            {
                audioSource.Value.volume = _BGMVolume;
            }
        }

        private void UpdateSXVolume()
        {
            Debug.Log("调整音效大小");
        }

        public void UpdateBGMVolume(Slider slider, TextMeshProUGUI volume)
        {
            BGMData.volume = slider.value;
            BGMVolume = BGMData.volume;
            volume.text = Math.Round(BGMData.volume * 100f).ToString() + "%";
        }

        public void UpdateSFXVolume(Slider slider, TextMeshProUGUI volume)
        {
            SFXData.volume = slider.value;
            SFXVolume = SFXData.volume;
            volume.text = Math.Round(SFXData.volume * 100f).ToString() + "%";
        }

        #endregion
    }
}