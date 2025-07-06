using UnityEngine;
using R3;
using TMPro;
using InGame.Scores;

namespace InGame.UI
{
    public class ScoreTextController : MonoBehaviour
    {
        TextMeshProUGUI _scoreText;

        void Start()
        {
            _scoreText = GetComponent<TextMeshProUGUI>();

            ScoreData.instance.Score
                .Subscribe(score =>
                {
                    UpdateScoreText(score);
                })
                .AddTo(this);
        }

        void UpdateScoreText(int score)
        {
            _scoreText.text = $"Score: {score}";
        }
    }
}
