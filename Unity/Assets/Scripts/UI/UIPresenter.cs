using R3;
using UnityEngine;

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
                    foreach (var display in _playingDisplay)
                    {
                        display.HandleDisplay(state);
                    }
                })
                .AddTo(this);
        }
    }

    public enum UIState
    {
        Playing,
        Paused,
        Finished
    }

    public class UIModel
    {
        ReactiveProperty<UIState> _uiState = new ReactiveProperty<UIState>(UIState.Playing);
        public ReadOnlyReactiveProperty<UIState> CurrentUIState => _uiState;

        public void SetUIState(UIState state)
        {
            _uiState.Value = state;
        }
    }
}
