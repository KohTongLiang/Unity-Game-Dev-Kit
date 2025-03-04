using System;
using UnityEngine;
using UnityServiceLocator;

namespace GameCore
{
    public enum WaypointType
    {
        Start,
        Finish
    }

    /// <summary>
    /// Generic walking quest objective template.
    /// </summary>
    public class WalkObjectiveWaypoint : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] protected WaypointType waypointType;
        [SerializeField] protected QuestSo questSo;
        [SerializeField] protected QuestObjectiveSo questObjectiveSo;
        [SerializeField] protected LayerMask playerLayer;

        [Header("Waypoint Indicator")] [SerializeField]
        private GameObject startIndicatorPrefab;

        [SerializeField] private GameObject endIndicatorPrefab;
        private GameObject _indicator;

        private QuestObjective _questObjective;

        private QuestManager questManager;
        private QuestObjective objective;

        private void Start()
        {
            questManager = QuestManager.Instance;
            questManager.OnQuestStarted += OnQuestStarted;
        }

        private void OnQuestStarted(Quest quest)
        {
            if (quest.QuestId != questSo.QuestId) return;

            questManager.GetQuestObjective(questSo.QuestId, questObjectiveSo.QuestObjectiveId, ref objective);
            if (objective == null) return;

            if (objective.ObjectiveState == QuestState.Unlocked)
            {
                if (waypointType == WaypointType.Start)
                {
                    enabled = true;
                    InitIndicator();
                }

                objective.OnObjectiveStart += OnObjectiveStart;
            }
        }

        private void OnObjectiveStart(int id)
        {
            objective.OnObjectiveStart -= OnObjectiveStart;

            // init end point
            if (waypointType == WaypointType.Finish)
            {
                InitIndicator();
                objective.OnObjectiveCompleted += OnObjectiveEnd;
                enabled = true;
                return;
            }

            // clean up start point
            Destroy(_indicator);
            enabled = false;
        }

        private void OnObjectiveEnd(int id)
        {
            objective.OnObjectiveCompleted -= OnObjectiveEnd;
            enabled = false;
            Destroy(_indicator);
        }

        private bool enabled = false;

        private void InitIndicator()
        {
            switch (waypointType)
            {
                case WaypointType.Start:
                    if (startIndicatorPrefab == null) return;
                    _indicator = Instantiate(startIndicatorPrefab, transform);
                    break;
                case WaypointType.Finish:
                    if (endIndicatorPrefab == null) return;
                    _indicator = Instantiate(endIndicatorPrefab, transform);
                    break;
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if ((playerLayer & (1 << other.gameObject.layer)) == 0 || !enabled) return;

            switch (waypointType)
            {
                case WaypointType.Start:
                    objective?.StartObjective();
                    break;
                case WaypointType.Finish:
                    objective?.CompleteObjective();
                    break;
            }
        }

        // Make a circular gizmos
        #if UNITY_EDITOR

        [Header("Editor Config")]
        public float GizmosRadius;

        private void OnDrawGizmos()
        {
            Gizmos.color = waypointType == WaypointType.Start ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position, GizmosRadius);
        }
        #endif
    }
}