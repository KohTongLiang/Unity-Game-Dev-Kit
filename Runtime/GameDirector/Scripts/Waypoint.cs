using UnityEngine;

namespace GameCore
{
     public enum WaypointType
     {
         Start,
         Finish,
         Trigger
     }

    public class Waypoint : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] protected WaypointType waypointType;
        [SerializeField] protected QuestSo questSo;
        [SerializeField] protected QuestObjectiveSo questObjectiveSo;
        [SerializeField] protected LayerMask playerLayer;

        [Header("Waypoint Indicator")]
        [SerializeField] protected GameObject indicatorPrefab;

        protected QuestObjective QuestObjective;
        protected QuestManager QuestManager;
        protected QuestObjective Objective;
        protected GameObject indicator;

        protected virtual void OnEnable()
        {
            QuestManager = QuestManager.Instance;
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
        }

        protected void OnTriggerExit(Collider other)
        {
        }

        protected virtual void InitIndicator()
        {
            indicator = Instantiate(indicatorPrefab, transform);
        }

        // Make a circular gizmos
        #if UNITY_EDITOR

        [Header("Editor Config")]
        public float GizmosRadius;
        public Color GizmosColor = Color.green;

        private void OnDrawGizmos()
        {
            Gizmos.color = GizmosColor;
            Gizmos.DrawSphere(transform.position, GizmosRadius);
        }
        #endif
    }
}