using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore
{
    public struct InteractionCallback
    {
        public int Priority { get; private set; }
        public bool Persistent { get; private set; }
        public Action Callback { get; private set; }

        public InteractionCallback(int priority, bool persistent, Action callback)
        {
            Priority = priority;
            Persistent = persistent;
            Callback = callback;
        }
    }

    /// <summary>
    /// Interaction callback Factory
    /// </summary>
    public class InteractionManager
    {
        private List<InteractionCallback> _callbacks = new();
        private readonly List<InteractionCallback> _persistentCallbacks = new();
        private InteractionCallback _highestPriorityCallback = new();

        #region Interface API

        /// <summary>
        /// Create a callback and pass in its priority and persistence. Returns the newly created interaction callback
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="priority"></param>
        /// <param name="persistent"></param>
        public InteractionCallback Bind(Action callback, int priority = 0, bool persistent = false)
        {
            InteractionCallback interactionCallback = _callbacks.Find(c => c.Callback == callback);
            if (_callbacks.Contains(interactionCallback))
            {
                return interactionCallback;
            }

            var newCallback = new InteractionCallback(priority, persistent, callback);
            _callbacks.Add(newCallback);

            if (persistent)
            {
                _persistentCallbacks.Add(newCallback);
            }
            else
            {
                _callbacks = _callbacks.OrderByDescending(c => c.Priority).ToList();
                _highestPriorityCallback = _callbacks.FirstOrDefault();
            }

            return newCallback;
        }

        /// <summary>
        /// Remove the interaction callback from either persistent or non-persistent callback list
        /// </summary>
        /// <param name="callback"></param>
        public void UnBind(InteractionCallback callback)
        {
            if (callback.Persistent)
            {
                _persistentCallbacks.Remove(callback);
            }
            else
            {
                _callbacks.Remove(callback);
            }

            _highestPriorityCallback = _callbacks.FirstOrDefault();
        }

        /// <summary>
        /// Clear all callbacks
        /// </summary>
        public void UnbindAll()
        {
            _callbacks.Clear();
            _persistentCallbacks.Clear();
        }

        /// <summary>
        /// Invoke the highest priority and all persistent callback
        /// </summary>
        public void InvokeCallbacks()
        {
            _highestPriorityCallback.Callback?.Invoke();
            _persistentCallbacks.ForEach(c => c.Callback?.Invoke());
        }

        #endregion
    }
}