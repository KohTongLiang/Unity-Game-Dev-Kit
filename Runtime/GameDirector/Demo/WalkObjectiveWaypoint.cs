using UnityEngine;
using UnityServiceLocator;

namespace GameCore
{
    /// <summary>
    /// Generic walking quest objective template.
    /// </summary>
    public class WalkObjectiveWaypoint : Waypoint
    {
        private bool enabled = false;

        private void Start()
        {
            QuestManager = ServiceLocator.For(this).Get<QuestManager>();
            QuestManager.OnQuestStarted += OnQuestStarted;
        }

        private void OnQuestStarted(Quest quest)
        {
            if (quest.QuestId != questSo.QuestId) return;

            QuestManager.GetQuestObjective(questSo.QuestId, questObjectiveSo.QuestObjectiveId, ref Objective);
            if (Objective == null) return;

            if (Objective.ObjectiveState == QuestState.Unlocked)
            {
                if (waypointType == WaypointType.Start)
                {
                    enabled = true;
                    InitIndicator();
                }

                Objective.OnObjectiveStart += OnObjectiveStart;
            }
        }

        private void OnObjectiveStart(QuestObjective objective)
        {
            Objective.OnObjectiveStart -= OnObjectiveStart;

            // init end point
            if (waypointType == WaypointType.Finish)
            {
                InitIndicator();
                Objective.OnObjectiveCompleted += OnObjectiveEnd;
                enabled = true;
                return;
            }

            // clean up start point
            Destroy(indicator);
            enabled = false;
        }

        private void OnObjectiveEnd(QuestObjective objective)
        {
            Objective.OnObjectiveCompleted -= OnObjectiveEnd;
            enabled = false;
            Destroy(indicator);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (LayerUtilities.CompareLayerToMask(playerLayer, other.gameObject.layer) || !enabled) return;

            switch (waypointType)
            {
                case WaypointType.Start:
                    Objective?.StartObjective();
                    break;
                case WaypointType.Finish:
                    Objective?.CompleteObjective();
                    break;
            }
        }
    }
}