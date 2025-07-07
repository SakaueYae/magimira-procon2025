using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Result
{
    public class CreditsDisplayToggle : MonoBehaviour
    {
        Subject<Unit> _onToggle = new Subject<Unit>();
        public Observable<Unit> OnToggle() => _onToggle;

        void Start()
        {
            this.GetComponent<Button>()
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _onToggle.OnNext(Unit.Default);
                })
                .AddTo(this);
        }
    }
}
