using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using InGame.Scores;
using TMPro;

namespace Result
{
    public class CalcScore : MonoBehaviour
    {
        [SerializeField] Image _fillImage;
        [SerializeField] TextMeshProUGUI _scoreText;
        [SerializeField] TextMeshProUGUI _messageText;

        string[] _messages = new string[]
        {
            "Great job!",
            "Well done!",
            "Keep it up!",
            "Fantastic!",
            "Excellent work!"
        };

        void Start()
        {
            int score = ScoreData.instance.Score.CurrentValue;
            int maxScore = ScoreData.instance.MaxScore;
            // エディターでのデバッグ用にスコアを固定
            // int score = 75; // 仮のスコア
            // int maxScore = 100; // 仮の最大スコア

            float percent = (float)score / maxScore;

            var sequence = DOTween.Sequence();
            sequence.Append(_fillImage.DOFillAmount(percent, 1f)
                .SetEase(Ease.InOutSine))
                .OnComplete(() =>
                {
                    Debug.Log("Fill complete!");
                    DisplayMessage(percent);
                });

            _scoreText.text = $"{score}/{maxScore}";
            _scoreText.color = new Color(_scoreText.color.r, _scoreText.color.g, _scoreText.color.b, 0f);
            sequence.Join(_scoreText.DOFade(1f, 1f)
                .SetEase(Ease.InOutSine));

            sequence.Play();
        }

        void DisplayMessage(float percent)
        {
            // スコアの割合に応じてメッセージを選択
            string selectedMessage = GetMessageByPercent(percent);

            // テキストを表示（フェードインアニメーション付き）
            if (_messageText != null)
            {
                _messageText.text = selectedMessage;
                _messageText.color = new Color(_messageText.color.r, _messageText.color.g, _messageText.color.b, 0f);
                _messageText.DOFade(1f, 0.5f);
            }
        }

        string GetMessageByPercent(float percent)
        {
            // パーセンテージに応じてメッセージを選択
            if (percent >= 0.9f)
                return _messages[4]; // "Excellent work!"
            else if (percent >= 0.7f)
                return _messages[3]; // "Fantastic!"
            else if (percent >= 0.5f)
                return _messages[2]; // "Keep it up!"
            else if (percent >= 0.3f)
                return _messages[1]; // "Well done!"
            else
                return _messages[0]; // "Great job!"
        }
    }
}
