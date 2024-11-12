using UnityEngine;

namespace GameCore
{
    public enum WaypointType
    {
        Start,
        Finish
    }

    /// <summary>
    /// Generic Start at A to B Waypoint
    /// </summary>
    public class Waypoint : MonoBehaviour
    {
        [Header("Config")] [SerializeField] protected WaypointType waypointType;
        [SerializeField] protected QuestSo questSo;
        [SerializeField] protected QuestObjectiveSo questObjectiveSo;

        [Header("Waypoint Indicator")] [SerializeField]
        private GameObject startIndicatorPrefab;

        [SerializeField] private GameObject endIndicatorPrefab;
        private GameObject _indicator;

        private QuestObjective _questObjective;
        protected bool Enabled = false;

        /// <summary>
        /// Get latest Quest Objective
        /// </summary>
        protected QuestObjective QuestObjective
        {
            get
            {
                var quest = QuestManager.Instance.GetQuest(questSo?.questAssetId);
                _questObjective = quest?.GetCurrentObjectives();
                return _questObjective;
            }
        }

        protected virtual void OnEnable()
        {
            InitIndicator();
            EventManager.Instance.RegisterListener<bool>(
                QuestEventConstants.ObjectiveWaypointEvent + questSo.questAssetId +
                questObjectiveSo.QuestObjectiveAssetId, ListenObjectiveUpdate);
        }

        protected virtual void OnDisable()
        {
            EventManager.Instance?.UnRegisterListener<bool>(
                QuestEventConstants.ObjectiveWaypointEvent + questSo.questAssetId +
                questObjectiveSo.QuestObjectiveAssetId, ListenObjectiveUpdate);

            Destroy(_indicator);
        }

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

        protected void ListenObjectiveUpdate(bool enableWaypoint)
        {
            _indicator?.SetActive(enableWaypoint);
            Enabled = enableWaypoint;
        }

        // TODO: Icons
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || !Enabled) return;
            switch (waypointType)
            {
                case WaypointType.Start:
                    // EventManager.Instance.TriggerEvent(EventConstants.StartObjectiveEvent, 0);
                    break;
                case WaypointType.Finish:
                    EventManager.Instance.TriggerEvent(QuestEventConstants.CompleteObjectiveEvent, 0);
                    break;
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
        }

        // Make a circular gizmos
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = waypointType == WaypointType.Start ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
        #endif
    }
}