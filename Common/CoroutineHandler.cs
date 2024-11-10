using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityServiceLocator;

namespace GameCore
{
    public class CoroutineHandler : MonoBehaviour
    {
        private Dictionary<string, IEnumerator> runningCoroutines = new();

        private void OnEnable()
        {
            ServiceLocator.Global.Register(this);
        }

        public IEnumerator StartHandlerCoroutine(string key, IEnumerator routine)
        {
            if (runningCoroutines.TryGetValue(key, out var coroutine))
            {
                Debug.LogWarning($"Coroutine with key {key} is already running. Shutting it down now.");
                StopHandlerCoroutine(key);
            }

            StartCoroutine(RunCoroutine(key, routine));
            runningCoroutines.Add(key, routine);
            return routine;
        }

        public void StopHandlerCoroutine(string key)
        {
            if (runningCoroutines.TryGetValue(key, out IEnumerator coroutine))
            {
                StopCoroutine(coroutine);
                runningCoroutines.Remove(key);
            }
            else
            {
                Debug.LogWarning($"Coroutine with key {key} is not running.");
            }
        }

        public void StopAllHandlerCoroutines()
        {
            foreach (var coroutine in runningCoroutines.Values)
            {
                StopCoroutine(coroutine);
            }

            runningCoroutines.Clear();
        }

        public bool IsCoroutineRunning(string key)
        {
            return runningCoroutines.ContainsKey(key);
        }

        private IEnumerator RunCoroutine(string key, IEnumerator routine)
        {
            yield return StartCoroutine(routine);
            runningCoroutines.Remove(key);
        }
    }
}