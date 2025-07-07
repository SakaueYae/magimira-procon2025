using UnityEngine;
using R3;

namespace Result
{
    public class CreditsDisplayHandle : MonoBehaviour
    {
        [SerializeField] CreditsDisplayToggle[] _creditsDisplayToggle;

        void Start()
        {
            gameObject.SetActive(false);
            foreach (var toggle in _creditsDisplayToggle)
            {
                toggle.OnToggle()
                    .Subscribe(_ =>
                    {
                        gameObject.SetActive(!gameObject.activeSelf);
                    })
                    .AddTo(this);
            }
        }
    }
}
