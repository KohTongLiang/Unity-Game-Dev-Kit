namespace GameCore
{
    public interface IPickable
    {
        public void OnHover();
        public void OnHoverLeave();
        public void OnPickup();
        public void OnDropped();
    }
}