using System;
using System.Collections.Generic;

namespace GameCore
{
    public class EventManager : Singleton<EventManager>
    {
        private readonly Dictionary<string, List<object>> _eventDictionary = new();

        public void RegisterListener(string eventName, Action listener)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.Add(listener);
            }
            else
            {
                _eventDictionary.Add(eventName, new List<object> { listener });
            }
        }

        public void RegisterListener<T>(string eventName, Action<T> listener)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.Add(listener);
            }
            else
            {
                _eventDictionary.Add(eventName, new List<object> { listener });
            }
        }

        public void UnRegisterListener(string eventName, Action listener)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.Remove(listener);
            }
        }

        public void UnRegisterListener<T>(string eventName, Action<T> listener)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.Remove(listener);
            }
        }

        public void TriggerEvent<T>(string eventName, T eventParam)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                List<object> invokeQueue = new(thisEvent);

                foreach (var action in invokeQueue)
                {
                    if (action is Action<T> typedAction)
                    {
                        typedAction.Invoke(eventParam);
                    }
                }
            }
        }
    }
}