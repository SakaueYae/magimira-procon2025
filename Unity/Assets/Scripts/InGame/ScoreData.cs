using R3;
using UnityEngine;

namespace InGame.Scores
{
    /// <summary>
    /// スコアデータを管理するシングルトンクラス
    /// </summary>
    public class ScoreData : MonoBehaviour
    {
        public static ScoreData instance;

        readonly ReactiveProperty<int> _score = new ReactiveProperty<int>(0);
        public ReadOnlyReactiveProperty<int> Score => _score;

        int _maxscore;
        public int MaxScore => _maxscore;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddScore()
        {
            _score.Value += 1;
        }

        public void SetMaxScore(int maxScore)
        {
            _maxscore = maxScore;
        }
    }

}
