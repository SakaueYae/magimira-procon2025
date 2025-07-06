using R3;
using R3.Triggers;
using InGame.Scores;
using UnityEngine;

namespace InGame.Player
{
    public class PlayerGetScores : MonoBehaviour
    {
        void Start()
        {
            this.OnTriggerEnter2DAsObservable()
                .Where(collision => collision.gameObject.TryGetComponent<ISongWord>(out var songWord))
                .Subscribe(collision =>
                {
                    ScoreData.instance.AddScore();
                    collision.gameObject.GetComponent<ISongWord>().GetWord();
                })
                .AddTo(this);
        }

        void Update()
        {

        }
    }
}
