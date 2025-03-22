using System;

namespace GameCore.UI
{
    [Serializable]
    public class BaseBinding
    {
        public string elementId;
    }

    [Serializable]
    public class LabelBinding : BaseBinding
    {
        public string dataKey;
    }

    public enum ButtonType
    {
        MountComponent,
        FireEvents,
        WriteDatastore
    }

    [Serializable]
    public struct ButtonEvent
    {
        public ButtonType type;
        public string dataKey;
        public string target;
    }

    [Serializable]
    public class ButtonBinding : BaseBinding
    {
        public ButtonEvent[] buttonEvents;
    }
}