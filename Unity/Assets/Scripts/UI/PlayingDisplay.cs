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
            if (state == UIState.Start)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                _pauseButton.interactable = state == _uiState;
            }
        }
    }
}
