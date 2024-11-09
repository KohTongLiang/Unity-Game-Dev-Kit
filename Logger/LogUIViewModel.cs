using UnityEngine;
using UnityEngine.UIElements;

namespace DeliveryGame
{
    public class LogUIViewModel : MonoBehaviour
    {
        [Header("Log UI")]
        [Tooltip("Reference the main UI document in this project. Include the LogUi visual tree asset in the main UI document.")]
        [SerializeField] private UIDocument logMessageUI;
        
        // [SerializeField] private GameEvent logEvent;

        private Button _toggleLogBtn;
        private Label _logLabel;
        private ScrollView _logScrollView;
        

        private void OnEnable()
        {
            var root = logMessageUI.rootVisualElement;
            
            _toggleLogBtn ??= root.Q<Button>("ToggleBtn");
            _logLabel ??= root.Q<Label>("LogLabel");
            _logScrollView ??= root.Q<ScrollView>("LogScrollView");

            _toggleLogBtn.clicked += ToggleLog;

            // if (logEvent is not null) logEvent.OnEventRaised += UpdateLogText;
        }

        private void OnDisable()
        {
            _toggleLogBtn.clicked -= ToggleLog;
            // if (logEvent is not null) logEvent.OnEventRaised -= UpdateLogText;
        }
        
        private void ToggleLog()
        {
            _logScrollView.style.display = _logScrollView.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void UpdateLogText(string message)
        {
            if (_logLabel is not null) _logLabel.text += message;
        }
    }
}
