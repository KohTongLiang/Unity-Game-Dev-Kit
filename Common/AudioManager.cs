using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityServiceLocator;

namespace GameCore
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }

        [SerializeField] private List<AudioClip> audioClips;
        [SerializeField] private int poolSize = 10;

        private readonly Dictionary<string, AudioClip> _audioClipDictionary = new();
        private readonly Dictionary<string, IEnumerator> _playingAudios = new();
        private readonly Dictionary<string, AudioSource> _playingAudiosource = new();
        private Queue<AudioSource> _audioPool;

        private CoroutineHandler coroutineHandler;

        private void OnEnable()
        {
            ServiceLocator.Global.Register(this);

            coroutineHandler = ServiceLocator.For(this).Get<CoroutineHandler>();
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

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

        public void Play(string clipName, bool playLoop = false)
        {
            if (!_audioClipDictionary.ContainsKey(clipName))
            {
                Debug.LogError($"Audio clip {clipName} not found");
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
            audioSource.volume = 1f;
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
    }
}