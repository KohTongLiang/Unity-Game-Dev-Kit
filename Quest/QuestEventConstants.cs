namespace GameCore
{
    public static class QuestEventConstants
    {
        public const string ObjectiveWaypointEvent = "ObjectiveWaypointEvent";
        public const string StartQuestEvent = "StartQuestEvent";
        public const string PickUpItemEvent = "PickUpItemEvent";
        public const string DeliveredItemEvent = "DeliveredItemEvent";
        
        // Objectives related event
        public const string StartObjectiveEvent = "StartObjectiveEvent";
        public const string UpdateObjectiveEvent = "UpdateObjectiveEvent";
        public const string CompleteObjectiveEvent = "CompleteObjectiveEvent";
        
        // UI Related
        public const string UpdateQuestUIInfoEvent = "StartObjectiveEvent";
        public const string UpdateObjectiveProgressEvent = "UpdateObjectiveEvent";
        public const string ClearUIEvent = "ClearUIEvent";
    }
}