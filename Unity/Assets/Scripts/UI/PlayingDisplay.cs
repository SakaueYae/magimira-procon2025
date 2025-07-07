using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayingDisplay : BaseUIDisplay
    {
        [SerializeField] Button _pauseButton;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        public override void HandleDisplay(UIState state)
        {
            _pauseButton.interactable = state == _uiState;
        }
    }
}
