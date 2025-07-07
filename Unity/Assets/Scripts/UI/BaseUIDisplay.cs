using UnityEngine;

namespace UI
{
    public class BaseUIDisplay : MonoBehaviour
    {
        [SerializeField] protected UIState _uiState;

        public virtual void HandleDisplay(UIState state)
        {
            gameObject.SetActive(state == _uiState);
        }

    }
}
