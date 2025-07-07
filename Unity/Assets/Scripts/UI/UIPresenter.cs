using R3;
using UnityEngine;
using WebGLBridge;

namespace UI
{
    public class UIPresenter : MonoBehaviour
    {
        [SerializeField] private UIStateToggle[] _uiStateToggle;
        [SerializeField] private BaseUIDisplay[] _playingDisplay;

        UIModel _uiModel;
        void Start()
        {
            _uiModel = new UIModel();

            // View -> Model
            foreach (var toggle in _uiStateToggle)
            {
                toggle.OnUIStateChanged()
                    .Subscribe(state => _uiModel.SetUIState(state))
                    .AddTo(this);
            }

            // Model -> View
            _uiModel.CurrentUIState
                .Subscribe(state =>
                {
                    Debug.Log($"Current UI State: {state}");
                    foreach (var display in _playingDisplay)
                    {
                        display.HandleDisplay(state);
                    }

                    if (state == UIState.Start || state == UIState.Paused)
                    {
                        Time.timeScale = 0f; // Pause the game
                        JSExecutor.PauseMusicFromJS(); // Pause music when paused
                    }
                    else
                    {
                        Time.timeScale = 1f; // Resume the game
                        JSExecutor.PlayMusicFromJS(); // Resume music when playing
                    }
                })
                .AddTo(this);
        }
    }

    public enum UIState
    {
        Start,
        Playing,
        Paused,
        Finished
    }

    public class UIModel
    {
        ReactiveProperty<UIState> _uiState = new ReactiveProperty<UIState>(UIState.Start);
        public ReadOnlyReactiveProperty<UIState> CurrentUIState => _uiState;

        public void SetUIState(UIState state)
        {
            _uiState.Value = state;
        }
    }
}
