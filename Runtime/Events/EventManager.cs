using System;
using System.Collections.Generic;

namespace GameCore
{
    public class EventManager : Singleton<EventManager>
    {
        private readonly Dictionary<int, List<object>> eventDictionary = new();

        public void RegisterListener(EventKey eventKey, Action listener)
        {
            if (eventDictionary.TryGetValue(eventKey.EventId, out var thisEvent))
                thisEvent.Add(listener);
            else
                eventDictionary.Add(eventKey.EventId, new List<object> { listener });
        }

        public void RegisterListener<T>(EventKey eventKey, Action<T> listener)
        {
            if (eventDictionary.TryGetValue(eventKey.EventId, out var thisEvent))
                thisEvent.Add(listener);
            else
                eventDictionary.Add(eventKey.EventId, new List<object> { listener });
        }

        public void UnRegisterListener(EventKey eventKey, Action listener)
        {
            if (eventDictionary.TryGetValue(eventKey.EventId, out var thisEvent)) thisEvent.Remove(listener);
        }

        public void UnRegisterListener<T>(EventKey eventKey, Action<T> listener)
        {
            if (eventDictionary.TryGetValue(eventKey.EventId, out var thisEvent)) thisEvent.Remove(listener);
        }


        public void TriggerEvent(EventKey eventKey)
        {
            if (eventDictionary.TryGetValue(eventKey.EventId, out var thisEvent))
            {
                List<object> invokeQueue = new(thisEvent);

                foreach (var action in invokeQueue)
                    if (action is Action typedAction)
                        typedAction.Invoke();
            }
        }

        public void TriggerEvent<T>(EventKey eventKey, T value)
        {
            if (eventDictionary.TryGetValue(eventKey.EventId, out var thisEvent))
            {
                List<object> invokeQueue = new(thisEvent);

                foreach (var action in invokeQueue)
                    if (action is Action<T> typedAction)
                        typedAction.Invoke(value);
            }
        }

        #region Old Functions

        public void RegisterListener(string eventName, Action listener)
        {
            var eventId = eventName.ComputeFNV1aHash();
            if (eventDictionary.TryGetValue(eventId, out var thisEvent))
                thisEvent.Add(listener);
            else
                eventDictionary.Add(eventId, new List<object> { listener });
        }

        public void RegisterListener<T>(string eventName, Action<T> listener)
        {
            var eventId = eventName.ComputeFNV1aHash();
            if (eventDictionary.TryGetValue(eventId, out var thisEvent))
                thisEvent.Add(listener);
            else
                eventDictionary.Add(eventId, new List<object> { listener });
        }

        public void UnRegisterListener(string eventName, Action listener)
        {
            var eventId = eventName.ComputeFNV1aHash();
            if (eventDictionary.TryGetValue(eventId, out var thisEvent)) thisEvent.Remove(listener);
        }

        public void UnRegisterListener<T>(string eventName, Action<T> listener)
        {
            var eventId = eventName.ComputeFNV1aHash();
            if (eventDictionary.TryGetValue(eventId, out var thisEvent)) thisEvent.Remove(listener);
        }

        public void TriggerEvent(string eventName)
        {
            var eventId = eventName.ComputeFNV1aHash();
            if (eventDictionary.TryGetValue(eventId, out var thisEvent))
            {
                List<object> invokeQueue = new(thisEvent);

                foreach (var action in invokeQueue)
                    if (action is Action typedAction)
                        typedAction.Invoke();
            }
        }

        public void TriggerEvent<T>(string eventName, T eventParam)
        {
            var eventId = eventName.ComputeFNV1aHash();
            if (eventDictionary.TryGetValue(eventId, out var thisEvent))
            {
                List<object> invokeQueue = new(thisEvent);

                foreach (var action in invokeQueue)
                    if (action is Action<T> typedAction)
                        typedAction.Invoke(eventParam);
            }
        }

        #endregion
    }
}