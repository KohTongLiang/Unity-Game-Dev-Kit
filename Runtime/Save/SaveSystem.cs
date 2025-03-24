using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace GameCore
{
  public enum MergePolicy
    {
        Overwrite,          // Always use new data
        KeepExisting,       // Always keep old data
        UseLatest,          // Compare timestamps
        MergeDictionaries,  // Special case for dictionaries
        Custom              // Use custom merge function
    }

    public struct SaveMetadata
    {
        public DateTime saveTime;
        public int version;
        public string sourceDevice;
    }

    public interface ISaveSystem
    {
        void Save<T>(string key, T data, MergePolicy policy = MergePolicy.UseLatest,
                    Func<T, T, T> customMerger = null) where T : new();

        T Load<T>(string key) where T : new();
        void Delete(string key);
        bool SaveExists(string key);
        SaveMetadata GetMetadata(string key);
    }
    
    public interface IMergeable<T>
    {
        T MergeWith(IMergeable<T> otherData);
    } 

    public class PlayerPrefsSaveSystem : ISaveSystem
    {
        private const string META_PREFIX = "_meta_";

        public void Save<T>(string key, T data, MergePolicy policy = MergePolicy.UseLatest,
                          Func<T, T, T> customMerger = null) where T : new()
        {
            var existingData = Load<T>(key);
            T mergedData;

            switch (policy)
            {
                case MergePolicy.Overwrite:
                    mergedData = data;
                    break;

                case MergePolicy.KeepExisting:
                    mergedData = existingData;
                    break;

                case MergePolicy.UseLatest:
                    mergedData = GetLatestData(data, existingData, key);
                    break;

                case MergePolicy.MergeDictionaries:
                    mergedData = MergeDictionaries(data, existingData);
                    break;

                case MergePolicy.Custom:
                    if (customMerger != null)
                    {
                        mergedData = customMerger(existingData, data);
                    }
                    else
                    {
                        Debug.LogWarning("Custom merge policy selected but no custom merger provided. Using default merge strategy.");
                        mergedData = DefaultMergeStrategy(data, existingData);
                    }
                    break;

                default:
                    mergedData = DefaultMergeStrategy(data, existingData);
                    break;
            }

            SaveInternal(key, mergedData);
            UpdateMetadata(key);
        }

        public T Load<T>(string key) where T : new()
        {
            if (!PlayerPrefs.HasKey(key)) return new T();

            var json = PlayerPrefs.GetString(key);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.DeleteKey(META_PREFIX + key);
            PlayerPrefs.Save();
        }

        public bool SaveExists(string key) => PlayerPrefs.HasKey(key);

        public SaveMetadata GetMetadata(string key)
        {
            var metaKey = META_PREFIX + key;
            return Load<SaveMetadata>(metaKey);
        }

        private T GetLatestData<T>(T newData, T existingData, string key)
        {
            var newMeta = new SaveMetadata { saveTime = DateTime.UtcNow };
            var existingMeta = GetMetadata(key);

            return newMeta.saveTime > existingMeta.saveTime ? newData : existingData;
        }

        private T MergeDictionaries<T>(T newData, T existingData)
        {
            if (newData is IDictionary newDict && existingData is IDictionary existingDict)
            {
                foreach (DictionaryEntry entry in newDict)
                {
                    existingDict[entry.Key] = entry.Value;
                }
                return (T)existingDict;
            }
            return newData;
        }

        private T DefaultMergeStrategy<T>(T newData, T existingData)
        {
            if (newData is IMergeable<T> mergeableNewData && existingData is IMergeable<T> mergeableExistingData)
            {
                return mergeableNewData.MergeWith(mergeableExistingData);
            }

            // Fallback: Use new data if types are not mergeable
            return newData;
        }

        private void SaveInternal<T>(string key, T data)
        {
            var json = JsonConvert.SerializeObject(data);
            Debug.Log($"Saving data to {key} {json}");
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        private void UpdateMetadata(string key)
        {
            var metaKey = META_PREFIX + key;
            var metadata = new SaveMetadata
            {
                saveTime = DateTime.UtcNow,
                version = 1,
                sourceDevice = SystemInfo.deviceUniqueIdentifier
            };

            SaveInternal(metaKey, metadata);
        }
    } 
}