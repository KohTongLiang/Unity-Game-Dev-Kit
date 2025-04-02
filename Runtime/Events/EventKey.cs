using UnityEngine;

namespace GameCore
{
    [CreateAssetMenu(fileName = "Event", menuName = "EventBus/EventKey", order = 0)]
    public class EventKey : ScriptableObject
    {
        public int EventId;
        public string EventName;

        #if UNITY_EDITOR

        private void OnValidate()
        {
            EventId = EventName.ComputeFNV1aHash();
        }
        #endif
    }
}