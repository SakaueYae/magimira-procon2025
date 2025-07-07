using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Result
{
    public class TransitionToTitle : MonoBehaviour
    {
        void Start()
        {
            this.GetComponent<Button>()
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SceneManager.LoadScene("GameScene");
                })
                .AddTo(this);
        }
    }
}
