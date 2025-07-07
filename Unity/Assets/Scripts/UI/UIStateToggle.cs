using R3;
using UnityEngine;
using UnityEngine.UI;
using WebGLBridge;

namespace UI
{
    public interface IUIStateToggle
    {
        Observable<UIState> OnUIStateChanged();
    }

    [RequireComponent(typeof(Button))]
    public class UIStateToggle : MonoBehaviour, IUIStateToggle
    {
        [SerializeField] UIState _uiState;

        readonly Subject<UIState> _uiStateSubject = new Subject<UIState>();
        public Observable<UIState> OnUIStateChanged() => _uiStateSubject;

        void Start()
        {
            this.GetComponent<Button>().OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _uiStateSubject.OnNext(_uiState);

                    if (Time.timeScale == 0f && _uiState == UIState.Playing)
                    {
                        JSExecutor.ResumeMusicFromJS(); // Resume music when playing
                        Time.timeScale = 1f; // Resume the game
                    }
                    else if (_uiState == UIState.Paused)
                    {
                        JSExecutor.PauseMusicFromJS(); // Pause music when paused
                        Time.timeScale = 0f; // Pause the game
                    }
                })
                .AddTo(this);
        }
    }

}