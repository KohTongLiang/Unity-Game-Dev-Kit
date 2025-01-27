using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private List<AudioClip> audioClips;
        [SerializeField] private int poolSize = 10;

        public float MasterVolume = 1f;

        private readonly Dictionary<string, AudioClip> _audioClipDictionary = new();
        private readonly Dictionary<string, IEnumerator> _playingAudios = new();
        private readonly Dictionary<string, AudioSource> _playingAudiosource = new();
        private Queue<AudioSource> _audioPool;

        private List<AudioSource> registeredAudio = new();

        private CoroutineHandler coroutineHandler;

        private void Awake()
        {
            foreach (var clip in audioClips)
            {
                _audioClipDictionary[clip.name] = clip;
            }
            
            _audioPool = new Queue<AudioSource>();

            for (var i = 0; i < poolSize; i++)
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                _audioPool.Enqueue(audioSource);
            }
        }

        private void OnEnable()
        {
            coroutineHandler ??= CoroutineHandler.Instance;
        }

        public void Play(string clipName, bool playLoop = false)
        {
            if (!_audioClipDictionary.ContainsKey(clipName))
            {
                Debug.LogError($"Audio clip {clipName} not found. Audio is skipped.");
                return;
            }

            AudioSource audioSource;
            if (_audioPool.Count > 0)
            {
                audioSource = _audioPool.Dequeue();
            }
            else
            {
                return;
            }
            
            if (_playingAudios.ContainsKey(clipName))
            {
                Debug.Log("Audio already playing");
                return;
            }

            audioSource.clip = _audioClipDictionary[clipName];
            audioSource.volume = MasterVolume;
            audioSource.loop = playLoop;
            audioSource.Play();

            var audioPlaying = coroutineHandler.StartHandlerCoroutine(clipName, EnqueueWhenFinished(clipName, audioSource));
            _playingAudios.Add(clipName, audioPlaying);
            _playingAudiosource.Add(clipName, audioSource);
        }

        public void Stop(string clipName)
        {
            if (!_playingAudios.TryGetValue(clipName, out var playingAudio)) return;
            coroutineHandler.StopHandlerCoroutine(clipName);

            var audioSource = _playingAudiosource[clipName];
            audioSource.Stop();
            
            _playingAudios.Remove(clipName);
            _playingAudiosource.Remove(clipName);
            _audioPool.Enqueue(audioSource);
        }

        private IEnumerator EnqueueWhenFinished(string clipName, AudioSource audioSource)
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);
            _playingAudios.Remove(clipName);
            _playingAudiosource.Remove(clipName);
            _audioPool.Enqueue(audioSource);
        }

        /// <summary>
        /// Public API for setting master volume of all the audio sources.
        /// </summary>
        /// <param name="volume"></param>
        public void SetMasterVolume (float volume)
        {
            MasterVolume = volume;
            foreach (var source in registeredAudio)
            {
                source.volume = volume;
            }
        }

        public void PlayLocalAudio(AudioSource audio, bool loop = false)
        {
            audio.volume = MasterVolume;
            if (audio.isPlaying) return;

            if (loop)
            {
                registeredAudio.Add(audio);
            }
            audio.loop = loop;
            audio.Play();
        }

        public void StopLocalAudio(AudioSource audio)
        {
            registeredAudio.Remove(audio);
            audio.Stop();
        }
    }
}