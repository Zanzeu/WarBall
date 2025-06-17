using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace WarBall.Common
{
    [CreateAssetMenu(menuName = "ÉùÒô/Êý¾Ý", fileName = "AudioData_")]
    public class AudioManagerSO : SerializedScriptableObject
    {
        [Range(0f, 1f)] public float volume;
        [Space(10)]
        [DictionaryDrawerSettings(KeyLabel = "Sounds", ValueLabel = "Clips")]
        public Dictionary<Sounds, AudioClip[]> clips;

        private void OnValidate()
        {
            volume = Mathf.Round(volume * 100f) / 100f;
        }
    }
}