using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using InGame.Scores;

namespace Result
{
    public class CalcPercent : MonoBehaviour
    {
        Image _image;

        void Start()
        {
            _image = GetComponent<Image>();
            int score = ScoreData.instance.Score.CurrentValue;
            int maxScore = ScoreData.instance.MaxScore;

            _image.DOFillAmount(0.5f, 1f)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => Debug.Log("Fill complete!"));
        }
    }
}
