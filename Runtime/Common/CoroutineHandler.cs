using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameCore
{
    public class CoroutineHandler : Singleton<CoroutineHandler>
    {
        private Dictionary<string, IEnumerator> runningCoroutines = new();

        #if UNITY_EDITOR
        public List<string> DebugRunningCoroutines => runningCoroutines.Keys.ToList();
        #endif

        public IEnumerator StartHandlerCoroutine(string key, IEnumerator routine)
        {
            if (runningCoroutines.TryGetValue(key, out var coroutine))
            {
                StopHandlerCoroutine(key);
            }

            StartCoroutine(RunCoroutine(key, routine));
            runningCoroutines.Add(key, routine);
            return routine;
        }

        /// <summary>
        /// Starts a coroutine if it is not already running. Returns whether coroutine was started.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="routine"></param>
        /// <returns></returns>
        public bool StartCoroutineIfNotRunning(string key, IEnumerator routine)
        {
            if (runningCoroutines.ContainsKey(key))
            {
                return false;
            }

            StartCoroutine(RunCoroutine(key, routine));
            runningCoroutines.Add(key, routine);
            return true;
        }

        public void StopHandlerCoroutine(string key)
        {
            if (!runningCoroutines.TryGetValue(key, out IEnumerator coroutine)) return;
            StopCoroutine(coroutine);
            runningCoroutines.Remove(key);
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