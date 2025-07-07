using UnityEngine;
using R3;
using DG.Tweening;

namespace Result
{
    public class CreditsDisplayHandle : MonoBehaviour
    {
        [SerializeField] CreditsDisplayToggle[] _creditsDisplayToggle;

        Vector3 _originalScale;

        void Start()
        {
            gameObject.SetActive(false);
            _originalScale = transform.localScale;

            foreach (var toggle in _creditsDisplayToggle)
            {
                toggle.OnToggle()
                    .Subscribe(_ =>
                    {
                        if (!gameObject.activeSelf)
                        {
                            gameObject.SetActive(true);
                            gameObject.transform.DOScale(transform.localScale * 1.2f, 0.5f)
                                .SetEase(Ease.OutBack)
                                .OnComplete(() =>
                                {
                                    gameObject.transform.DOScale(transform.localScale, 0.5f)
                                        .SetEase(Ease.InBack);
                                });
                        }
                        else
                        {
                            gameObject.transform.DOScale(transform.localScale * 0.8f, 0.5f)
                                .SetEase(Ease.InBack)
                                .OnComplete(() =>
                                {
                                    transform.localScale = _originalScale; // Reset scale
                                    gameObject.SetActive(false);
                                });
                        }

                    })
                    .AddTo(this);
            }
        }
    }
}
