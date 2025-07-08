using R3;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                
                // シーン変更時のイベントを購読
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void OnDestroy()
        {
            // イベントの購読を解除（メモリリーク防止）
            if (instance == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        /// <summary>
        /// シーンが読み込まれた時に呼ばれる
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // GameSceneに遷移した時にスコアをリセット
            if (scene.name == "GameScene")
            {
                ResetScore();
                Debug.Log("Score reset for GameScene");
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

        /// <summary>
        /// スコアをリセットする
        /// </summary>
        public void ResetScore()
        {
            _score.Value = 0;
            Debug.Log("Score has been reset to 0");
        }
    }

}
