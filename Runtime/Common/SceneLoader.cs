using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace GameCore
{
    public class SceneLoader
    {
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> sceneLoaded = new();

        /// <summary>
        /// Load in a scene using its addressable address.
        /// </summary>
        /// <param name="sceneAddress">Addressable address</param>
        /// <param name="origin">The caller of the function</param>
        /// <param name="mode">Single, Additive etc.</param>
        /// <param name="setSceneActive">Specify to set the newly loaded scene as active</param>
        /// <param name="sceneLoadedCallback">Callback that will be fired once the scene is loaded successfully.</param>
        public void LoadScene(
            string sceneAddress,
            string origin,
            LoadSceneMode mode,
            bool setSceneActive = false,
            Action sceneLoadedCallback = null)
        {
            if (sceneLoaded.ContainsKey(sceneAddress + origin))
            {
                return;
            }

            // Load the scene using Addressables
            AsyncOperationHandle<SceneInstance> assetOperation = Addressables.LoadSceneAsync(sceneAddress, mode);
            assetOperation.Completed += obj => OnSceneLoaded(obj, sceneAddress + origin, setSceneActive, sceneLoadedCallback);
        }

        private void OnSceneLoaded(
            AsyncOperationHandle<SceneInstance> obj,
            string sceneAddress,
            bool setSceneActive = false,
            Action sceneLoadedCallback = null)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Scene successfully loaded!");
                sceneLoadedCallback?.Invoke();
                if (setSceneActive) SceneManager.SetActiveScene(obj.Result.Scene);
                sceneLoaded.Add(sceneAddress, obj);
            }
            else
            {
                // Remove the scene from the dictionary if it failed to load
                Debug.LogError("Failed to load scene.");
            }
        }

        /// <summary>
        /// Call to unload the scene asynchronously
        /// </summary>
        /// <param name="sceneAddress"></param>
        /// <param name="origin"></param>
        public void UnloadScene(string sceneAddress, string origin)
        {
            if (!sceneLoaded.TryGetValue(sceneAddress + origin, out var assetOperation)) return;
            Addressables.UnloadSceneAsync(assetOperation).Completed += handle => OnSceneUnloaded(handle, sceneAddress + origin);
        }

        // Callback once the scene is unloaded
        private void OnSceneUnloaded(AsyncOperationHandle<SceneInstance> obj, string sceneAddress)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                sceneLoaded.Remove(sceneAddress);
                Debug.Log("Scene successfully unloaded!");
            }
            else
            {
                Debug.LogError("Failed to unload scene.");
            }
        }
    }
}