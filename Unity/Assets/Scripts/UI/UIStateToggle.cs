using R3;
using UnityEngine;
using UnityEngine.UI;

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
                })
                .AddTo(this);
        }
    }

}