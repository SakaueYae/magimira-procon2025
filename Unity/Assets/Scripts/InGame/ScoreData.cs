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

        void Start()
        {

        }

        public void AddScore()
        {
            _score.Value += 1;
        }
    }

}
