using UnityEngine.UIElements;

namespace GameCore.UI
{
    /// <summary>
    /// For UI Components that requires to be overlayed on other UI components (eg. a Modal, a Toast message).
    /// </summary>
    public class OverlayViewModel : RootViewModel
    {
        public override void Mount()
        {
            if (Active) return;
            Active = true;
            GameContentContainer = UiManager.Instance.rootDocument.rootVisualElement.Q<VisualElement>("OverlayContent");
            container = UIAsset.Instantiate();
            UIComponent = container.contentContainer;
            UIComponent.style.flexGrow = new StyleFloat(templateContainerFlexGrow);
            GameContentContainer.contentContainer.Add(UIComponent);
            GameContentContainer.style.visibility = Visibility.Visible;
        }

        public override void DisMount()
        {
            base.DisMount();
            GameContentContainer.style.visibility = Visibility.Hidden;
        }
    }
}