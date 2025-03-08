namespace GameCore.UI
{
    public struct Popup<T>
    {
        public bool persistent { get; private set; }
        public float duration { get; private set; }
        public T popupContent { get; private set; }

        public Popup(bool persistent, float duration, T popupContent)
        {
            this.persistent = persistent;
            this.duration = duration;
            this.popupContent = popupContent;
        }
    }

    public static class PopupFactory
    {
        public static Popup<T> BuildPopup<T>(bool persistent, float duration, T content)
        {
            return new Popup<T>(persistent, duration, content);
        }
    }
}