using UnityEngine;
using R3;
using WebGLBridge;

namespace InGame
{
    public class DataTransmitter : MonoBehaviour
    {
        [SerializeField] JSMessageReceiver _jsMessageReceiver;

        [SerializeField] SongWordController _wordControllerPrefab;
        [SerializeField] BouncingBallController _ballController;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // _textAliveManager.WordStream()
            //     .Subscribe(word =>
            //     {
            //         if (!string.IsNullOrEmpty(word))
            //         {
            //             Vector3 _currentPos = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0f);
            //             SongWordController songWord = Instantiate(_wordControllerPrefab, _currentPos, Quaternion.identity);
            //             songWord.SetWord(word);
            //         }
            //     })
            //     .AddTo(this);

            _jsMessageReceiver.BeatStream()
                .Subscribe(interval =>
                {
                    _ballController.SetBounceInterval(interval / 1000f);
                    Debug.Log("Bounce interval set to: " + interval);
                })
                .AddTo(this);

            _jsMessageReceiver.IsPlaying
                .Subscribe((isPlaying) =>
                {
                    _ballController.SwitchBounce(isPlaying);
                })
                .AddTo(this);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
