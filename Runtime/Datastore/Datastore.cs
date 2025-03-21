using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameCore
{
    [Serializable]
    public readonly struct DatastoreKey : IEquatable<DatastoreKey>
    {
        public readonly string Key;
        public readonly int HashedKey;

        public DatastoreKey(string key)
        {
            Key = key;
            HashedKey = key.ComputeFNV1aHash();
        }

        public bool Equals(DatastoreKey other) => HashedKey == other.HashedKey;
        public override bool Equals(object obj) => obj is DatastoreKey other && Equals(other);

        // Essential to override as Dictionary will use this method and we want it to use our own hashed key
        public override int GetHashCode() => HashedKey;
        public override string ToString() => Key;

        public static bool operator ==(DatastoreKey lhs, DatastoreKey rhs) => lhs.HashedKey == rhs.HashedKey;
        public static bool operator !=(DatastoreKey lhs, DatastoreKey rhs) => !(lhs == rhs);
    }

    [Serializable]
    public struct DatastoreEntity<T>
    {
        private DatastoreKey Key { get; }
        public T Value { get; private set; }
        public Type ValueType { get; }
        public Action<T> Callbacks { get; set; }

        public DatastoreEntity(DatastoreKey key, T value, Action<T> callbacks = null)
        {
            Key = key;
            Value = value;
            ValueType = typeof(T);
            Callbacks = callbacks;
        }

        public void OnValueChanged()
        {
            Callbacks?.Invoke(Value);
        }

        public void AssignData(T updatedValue, Action<T> callbacks = null)
        {
            Value = updatedValue;
            if (callbacks is not null) Callbacks += callbacks;
            OnValueChanged();
        }

        public override bool Equals(object obj) => obj is DatastoreEntity<T> other && Equals(other);
        public override int GetHashCode() => Key.GetHashCode();
        public override string ToString()
        {
            return $"Key: {Key.ToString()}, Value: {JsonConvert.SerializeObject(Value)}";
        }
    }

    public class Datastore
    {
        private readonly Dictionary<string, DatastoreKey> keyRegistry = new();
        private readonly Dictionary<string, Dictionary<DatastoreKey, object>> datastore = new();

        public DatastoreKey GetOrRegisterKey(string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
            {
                keyName = $"{DateTime.Now}";
            }

            if (!keyRegistry.TryGetValue(keyName, out var key))
            {
                key = new DatastoreKey(keyName);
                keyRegistry.Add(keyName, key);
            }

            return key;
        }

        /// <summary>
        /// Attempt to retrieve a datastore entry via a key, return false if not found, sets out value if found
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetValue<T>(string keyName, out T value, string type = "default")
        {
            DatastoreKey key = GetOrRegisterKey(keyName);
            datastore.TryGetValue(type, out var datastoreEntries);
            if (datastoreEntries is not null && datastoreEntries.TryGetValue(key, out var entry) && entry is DatastoreEntity<T> castedEntry)
            {
                value = castedEntry.Value;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Attempt to retrieve a datastore entry via a key, return false if not found, sets out value if found
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetReference<T>(string keyName, ref T value, string type = "default")
        {
            var key = GetOrRegisterKey(keyName);
            datastore.TryGetValue(type, out var datastoreEntries);
            if (datastoreEntries is not null && datastoreEntries.TryGetValue(key, out var entry) &&
                entry is DatastoreEntity<T> castedEntry)
            {
                value = castedEntry.Value;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Adds a new entry to datastoreEntries or update existing entry. Returns key for future references.
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        /// <param name="callback"></param>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        public void AddOrUpdate<T>(string keyName, T value, Action<T> callback = null, string type = "default")
        {
            DatastoreKey key = GetOrRegisterKey(keyName);
            datastore.TryGetValue(type, out var datastoreEntries);
            if (datastoreEntries is null)
            {
                datastoreEntries = new Dictionary<DatastoreKey, object>();
                datastore.Add(type, datastoreEntries);
            }

            if (datastoreEntries.TryGetValue(key, out var entry) && entry is DatastoreEntity<T> castedEntry)
            {
                castedEntry.AssignData(value, callback);
                datastoreEntries[key] = castedEntry;
            }
            else
            {
                datastoreEntries.Add(key, new DatastoreEntity<T>(key, value, callback));
                callback?.Invoke(value);
            }
        }

        public void Remove(string keyName, string type = "default")
        {
            var key = GetOrRegisterKey(keyName);
            datastore.TryGetValue(type, out var datastoreEntries);
            if (datastoreEntries is null)
            {
                return;
            }

            keyRegistry.Remove(key.Key);
            datastoreEntries.Remove(key);
        }

        public void RegisterCallback<T>(string keyName, Action<T> callback, string type = "default", bool init = false)
        {
            DatastoreKey key = GetOrRegisterKey(keyName);
            datastore.TryGetValue(type, out var datastoreEntries);
            if (datastoreEntries is null)
            {
                datastoreEntries = new Dictionary<DatastoreKey, object>();
                datastore.Add(type, datastoreEntries);
            }

            if (datastoreEntries.TryGetValue(key, out var entry) && entry is DatastoreEntity<T> castedEntry)
            {
                castedEntry.Callbacks += callback;
                datastoreEntries[key] = castedEntry;
                if (init) castedEntry.Callbacks?.Invoke(castedEntry.Value);

            }
            else
            {
                datastoreEntries.Add(key, new DatastoreEntity<T>(key, default, callback));
                if (init) callback?.Invoke(default);
            }
        }

        public void UnregisterCallback<T>(string keyName, Action<T> callback, string type = "default")
        {
            DatastoreKey key = GetOrRegisterKey(keyName);
            datastore.TryGetValue(type, out var datastoreEntries);
            if (datastoreEntries is null)
            {
                return;
            }

            if (datastoreEntries.TryGetValue(key, out var entry) && entry is DatastoreEntity<T> castedEntry)
            {
                castedEntry.Callbacks -= callback;
                datastoreEntries[key] = castedEntry;
            }
        }
    }
}